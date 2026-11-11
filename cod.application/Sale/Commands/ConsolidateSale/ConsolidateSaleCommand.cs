using cod.application.Common.Clases;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.domain.Common;
using cod.domain.Features;
using cod.domain.Finance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Sale.Commands.ConsolidateSale
{
    public class ConsolidateSaleCommand : BaseCommand, IConsolidateSaleCommand
    {
        private readonly AuthService _authService;

        public ConsolidateSaleCommand(
            IRepository repository,
            IExceptionFactory exceptionFactory,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task Execute(ConsolidateSaleModel model)
        {
            model = model ?? new ConsolidateSaleModel();
            var timeZone = _authService.ClientTimezone;
            if (model.SaleDate != DateTime.MinValue)
                model.SaleDate = DateService.GetCLientLocalTime(model.SaleDate, timeZone);

            var saleIds = (model.SaleIds ?? new List<int>()).Where(id => id > 0).Distinct().ToList();
            var lines = (model.Lines ?? new List<ConsolidateSaleLineModel>())
                .Where(line => line != null && line.Quantity > 0)
                .GroupBy(line => new { line.ProductId, line.Price })
                .Select(group => new ConsolidateSaleLineModel
                {
                    ProductId = group.Key.ProductId,
                    Price = group.Key.Price,
                    Quantity = group.Sum(line => line.Quantity)
                })
                .ToList();

            if (model.RouteId <= 0)
                this._exceptionFactory.Add("route", this._exceptionFactory.GetMessage("Ruta", ExceptionEnums.ExceptionCode.Required));
            if (model.CustomerId <= 0)
                this._exceptionFactory.Add("customerId", this._exceptionFactory.GetMessage("Cliente", ExceptionEnums.ExceptionCode.Required));
            if (model.SaleDate == DateTime.MinValue)
                this._exceptionFactory.Add("saleDate", this._exceptionFactory.GetMessage("Fecha de Venta", ExceptionEnums.ExceptionCode.Required));
            if (!saleIds.Any())
                this._exceptionFactory.Add("saleIds", "Seleccione al menos una venta");
            if (!lines.Any())
                this._exceptionFactory.Add("lines", "Indique cuántos botes sacar");
            if (lines.Any(line => line.Price <= 0))
                this._exceptionFactory.Add("lines", "Solo se sacan productos con precio mayor a 0");

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "VENTAS", this._exceptionFactory.Exceptions);

            var customerExists = await _repository.AnyActive<domain.Data.Customer>(customer => customer.Id == model.CustomerId);
            if (!customerExists)
                this._exceptionFactory.Add("customerId", this._exceptionFactory.GetMessage("Cliente", ExceptionEnums.ExceptionCode.Required));

            var nowDate = DateService.GetCLientLocalTime(Clock.Now, timeZone);
            var correlative = await _repository.AllActive<domain.Data.Correlative>()
                .Where(item => item.StartDate.Date <= nowDate.Date && item.EndDate.Date >= nowDate.Date)
                .Where(item => item.Current < item.End)
                .FirstOrDefaultAsync();
            if (correlative == null)
                throw new ApplicationException("CANCELED", "CORRELATIVO NO CONFIGURADO", this._exceptionFactory.Exceptions);

            var sales = await _repository.WhereAsync<domain.Operation.Sale>(sale => saleIds.Contains(sale.Id))
                .Include(sale => sale.SaleDetails)
                    .ThenInclude(detail => detail.Product)
                .Include(sale => sale.Route)
                .ToListAsync();

            if (sales.Count != saleIds.Count)
                this._exceptionFactory.Add("saleIds", "Hay ventas que no existen");
            if (sales.Any(sale => sale.RouteId != model.RouteId))
                this._exceptionFactory.Add("route", "Las ventas tienen que ser de la misma ruta");
            if (sales.Any(sale => !sale.IsActive || sale.SaleType != SaleType.Cash || sale.SaleInvoiceId.HasValue))
                this._exceptionFactory.Add("saleIds", "Solo se sacan botes de ventas activas, al contado y sin factura");

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "VENTAS", this._exceptionFactory.Exceptions);

            var taken = new List<(domain.Operation.SaleDetail Detail, int Quantity, bool Tax)>();
            foreach (var line in lines)
            {
                var sources = sales
                    .SelectMany(sale => (sale.SaleDetails ?? new List<domain.Operation.SaleDetail>())
                        .Where(detail => detail.IsActive
                            && detail.Quantity > 0
                            && detail.Price > 0
                            && detail.ProductId == line.ProductId
                            && detail.Price == line.Price)
                        .Select(detail => new SourceLine
                        {
                            Sale = sale,
                            Detail = detail,
                            Available = detail.Quantity
                        }))
                    .ToList();

                var available = sources.Sum(source => source.Available);
                var productName = sources.Select(source => source.Detail.Product?.Name).FirstOrDefault(name => !string.IsNullOrWhiteSpace(name))
                    ?? "producto";
                if (available < line.Quantity)
                    throw new ApplicationException(
                        "CANCELED",
                        $"No alcanzan los botes de {productName} a {line.Price}. Hay {available} y se pidieron {line.Quantity}.",
                        this._exceptionFactory.Exceptions);

                var shares = ShareAcrossSales(sources, line.Quantity);
                foreach (var share in shares)
                {
                    var tax = share.Detail.Product != null && share.Detail.Product.Tax;
                    taken.Add((share.Detail, share.Quantity, tax));
                }
            }

            var database = ((DbContext)_repository).Database;
            await using var transaction = await database.BeginTransactionAsync();
            try
            {
                foreach (var share in taken)
                {
                    share.Detail.Quantity -= share.Quantity;
                    share.Detail.Total = share.Detail.Price * share.Detail.Quantity;
                    if (share.Detail.Quantity <= 0)
                    {
                        share.Detail.Quantity = 0;
                        share.Detail.Total = 0;
                        share.Detail.IsActive = false;
                    }
                }

                foreach (var sale in sales)
                {
                    var remaining = (sale.SaleDetails ?? new List<domain.Operation.SaleDetail>())
                        .Where(detail => detail.IsActive && detail.Quantity > 0)
                        .ToList();
                    sale.Amount = remaining.Sum(detail => detail.Total);
                    if (!remaining.Any())
                    {
                        sale.IsActive = false;
                        sale.DisableDateTime = Clock.Now;
                        sale.DisableUser = _authService.User;
                    }
                }

                var route = sales.First().Route;
                var newDetails = taken
                    .GroupBy(item => new { item.Detail.ProductId, item.Detail.Price, item.Tax })
                    .Select(group => new domain.Operation.SaleDetail
                    {
                        ProductId = group.Key.ProductId,
                        Price = group.Key.Price,
                        Quantity = group.Sum(item => item.Quantity),
                        Total = group.Key.Price * group.Sum(item => item.Quantity),
                        IsActive = true,
                        CreationTime = Clock.Now,
                        CreatorUser = _authService.User
                    })
                    .ToList();

                var consolidated = new domain.Operation.Sale
                {
                    RouteId = model.RouteId,
                    Vendor = route == null || string.IsNullOrWhiteSpace(route.Vendor) ? string.Empty : route.Vendor.Trim(),
                    CustomerId = model.CustomerId,
                    SaleType = SaleType.Cash,
                    SaleDate = model.SaleDate,
                    Amount = newDetails.Sum(detail => detail.Total),
                    SaleDetails = newDetails
                };

                _repository.InsertAsync(consolidated);
                await _repository.CommitAsync();

                var subTotalTax = taken.Where(item => item.Tax).Sum(item => item.Detail.Price * item.Quantity);
                var subTotalExempt = taken.Where(item => !item.Tax).Sum(item => item.Detail.Price * item.Quantity);
                var subTotal = subTotalTax + subTotalExempt;
                var taxAmount = Math.Round(subTotalTax * 0.15, 2);
                var invoice = new SaleInvoice
                {
                    InvoiceDate = model.SaleDate,
                    PaymentType = PaymentType.Cash,
                    PaymentStatus = PaymentStatus.Paid,
                    Balance = 0,
                    Paid = true,
                    PaidAt = Clock.Now,
                    PaidUser = _authService.User,
                    TaxableAmount18 = 0,
                    Discount = 0,
                    Tax18Amount = 0,
                    DueDate = null,
                    CustomerId = model.CustomerId,
                    TaxableAmount = subTotalTax,
                    ExemptAmount = subTotalExempt,
                    SubTotal = subTotal,
                    TaxAmount = taxAmount,
                    Total = subTotal + taxAmount,
                    CorrelativeId = correlative.Id,
                    InvoiceNumber = $"{correlative.EmissionPoint}-{correlative.Establishment}-{correlative.DocumentType}-{correlative.Current.ToString("D8")}",
                    Details = new List<SaleInvoiceDetail>
                    {
                        new SaleInvoiceDetail
                        {
                            SaleId = consolidated.Id,
                            IsActive = true,
                            CreationTime = Clock.Now,
                            CreatorUser = _authService.User
                        }
                    }
                };

                correlative.Current += 1;
                _repository.InsertAsync(invoice);
                await _repository.CommitAsync();

                consolidated.SaleInvoiceId = invoice.Id;
                await _repository.CommitAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private static List<SourceLine> ShareAcrossSales(List<SourceLine> sources, int requested)
        {
            var sales = sources
                .GroupBy(source => source.Sale.Id)
                .Select(group => new SaleShare
                {
                    SaleId = group.Key,
                    Available = group.Sum(source => source.Available),
                    Lines = group.ToList()
                })
                .Where(sale => sale.Available > 0)
                .OrderByDescending(sale => sale.Available)
                .ToList();

            var takeBySale = sales.ToDictionary(sale => sale.SaleId, sale => 0);
            if (requested >= sales.Count)
            {
                foreach (var sale in sales)
                    takeBySale[sale.SaleId] = 1;

                var left = requested - sales.Count;
                var capacity = sales
                    .Select(sale => new Capacity { SaleId = sale.SaleId, Cap = sale.Available - 1 })
                    .Where(item => item.Cap > 0)
                    .ToList();
                AddProportional(capacity, left, takeBySale);
            }
            else
            {
                foreach (var sale in sales.Take(requested))
                    takeBySale[sale.SaleId] = 1;
            }

            var result = new List<SourceLine>();
            foreach (var sale in sales)
            {
                var remaining = takeBySale[sale.SaleId];
                foreach (var line in sale.Lines)
                {
                    if (remaining <= 0)
                        break;
                    var quantity = Math.Min(line.Available, remaining);
                    if (quantity > 0)
                    {
                        result.Add(new SourceLine
                        {
                            Sale = line.Sale,
                            Detail = line.Detail,
                            Quantity = quantity
                        });
                    }
                    remaining -= quantity;
                }
            }

            return result;
        }

        private static void AddProportional(List<Capacity> capacity, int left, Dictionary<int, int> takeBySale)
        {
            if (left <= 0)
                return;

            var capSum = capacity.Sum(item => item.Cap);
            var pieces = capacity.Select(item =>
            {
                var exact = capSum == 0 ? 0 : (double)item.Cap / capSum * left;
                var floor = (int)Math.Floor(exact);
                return new
                {
                    item.SaleId,
                    item.Cap,
                    Floor = Math.Min(floor, item.Cap),
                    Fraction = exact - floor
                };
            }).ToList();

            var assigned = pieces.ToDictionary(item => item.SaleId, item => item.Floor);
            var remainder = left - assigned.Values.Sum();
            foreach (var item in pieces.OrderByDescending(item => item.Fraction).ThenByDescending(item => item.Cap))
            {
                if (remainder <= 0)
                    break;
                if (assigned[item.SaleId] < item.Cap)
                {
                    assigned[item.SaleId] += 1;
                    remainder -= 1;
                }
            }

            foreach (var item in pieces.OrderByDescending(item => item.Cap - assigned[item.SaleId]))
            {
                while (remainder > 0 && assigned[item.SaleId] < item.Cap)
                {
                    assigned[item.SaleId] += 1;
                    remainder -= 1;
                }
            }

            foreach (var item in assigned)
                takeBySale[item.Key] += item.Value;
        }

        private class SourceLine
        {
            public domain.Operation.Sale Sale { get; set; }
            public domain.Operation.SaleDetail Detail { get; set; }
            public int Available { get; set; }
            public int Quantity { get; set; }
        }

        private class SaleShare
        {
            public int SaleId { get; set; }
            public int Available { get; set; }
            public List<SourceLine> Lines { get; set; }
        }

        private class Capacity
        {
            public int SaleId { get; set; }
            public int Cap { get; set; }
        }
    }
}
