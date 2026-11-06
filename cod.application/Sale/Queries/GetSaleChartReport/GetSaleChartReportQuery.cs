using cod.application.Common.Clases;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.Sale.Queries.GetSaleChartReport
{
    public class GetSaleChartReportQuery : BaseQuery, IGetSaleChartReportQuery
    {
        private readonly AuthService _authService;

        public GetSaleChartReportQuery(IRepository repository, AuthService authService) : base(repository)
        {
            _authService = authService;
        }

        public async Task<List<SaleChartPointModel>> Execute(SaleChartFilterModel filter)
        {
            filter = filter ?? new SaleChartFilterModel();
            var sales = FilteredSales(filter);
            switch ((filter.Report ?? string.Empty).Trim().ToLower())
            {
                case "vendor":
                    return await VendorReport(sales);
                case "product":
                    return await ProductReport(sales);
                case "product-price":
                    return await ProductPriceReport(sales, filter.Product);
                case "payment":
                    return await PaymentReport(sales);
                default:
                    return new List<SaleChartPointModel>();
            }
        }

        private IQueryable<domain.Operation.Sale> FilteredSales(SaleChartFilterModel filter)
        {
            var today = Clock.Now;
            var from = filter.DateFrom ?? new DateTime(today.Year, today.Month, 1);
            var to = filter.DateTo ?? today;
            var timezone = _authService.ClientTimezone;
            from = DateService.GetCLientLocalTime(from, timezone).Date;
            to = DateService.GetCLientLocalTime(to, timezone).Date;

            return _repository.QueryActive<domain.Operation.Sale>()
                .Where(sale => sale.SaleDate.Date >= from && sale.SaleDate.Date <= to);
        }

        private static async Task<List<SaleChartPointModel>> VendorReport(IQueryable<domain.Operation.Sale> sales)
        {
            return await sales
                .Where(sale => sale.Route.Name.ToLower() != "planta"
                    && sale.Vendor != null
                    && sale.Vendor != "")
                .GroupBy(sale => sale.Vendor)
                .Select(group => new SaleChartPointModel
                {
                    Label = group.Key,
                    Series = "Monto",
                    Amount = Math.Round(group.Sum(sale => sale.Amount), 2),
                    Quantity = group.Count()
                })
                .OrderByDescending(point => point.Amount)
                .ToListAsync();
        }

        private static async Task<List<SaleChartPointModel>> ProductReport(IQueryable<domain.Operation.Sale> sales)
        {
            var details = sales.SelectMany(sale => sale.SaleDetails).Where(detail => detail.IsActive);

            var sold = await details
                .Where(detail => detail.Price > 0)
                .GroupBy(detail => detail.Product.Name)
                .Select(group => new SaleChartPointModel
                {
                    Label = group.Key,
                    Series = "Venta",
                    Amount = Math.Round(group.Sum(detail => detail.Total), 2),
                    Quantity = group.Sum(detail => detail.Quantity)
                })
                .OrderByDescending(point => point.Amount)
                .ToListAsync();

            var regalia = await details
                .Where(detail => detail.Price == 0)
                .GroupBy(detail => detail.Product.Name)
                .Select(group => new SaleChartPointModel
                {
                    Label = group.Key,
                    Series = "Regalía",
                    Amount = 0,
                    Quantity = group.Sum(detail => detail.Quantity)
                })
                .OrderByDescending(point => point.Quantity)
                .ToListAsync();

            sold.AddRange(regalia);
            return sold;
        }

        private static async Task<List<SaleChartPointModel>> ProductPriceReport(IQueryable<domain.Operation.Sale> sales, string product)
        {
            var name = (product ?? string.Empty).Trim();
            if (name.Length == 0)
                return new List<SaleChartPointModel>();

            var rows = await sales
                .SelectMany(sale => sale.SaleDetails)
                .Where(detail => detail.IsActive && detail.Product.Name == name)
                .GroupBy(detail => detail.Price)
                .Select(group => new
                {
                    Price = group.Key,
                    Amount = group.Sum(detail => detail.Total),
                    Quantity = group.Sum(detail => detail.Quantity)
                })
                .OrderBy(row => row.Price)
                .ToListAsync();

            return rows.Select(row => new SaleChartPointModel
            {
                Label = row.Price.ToString(System.Globalization.CultureInfo.InvariantCulture),
                Series = "Precio",
                Amount = Math.Round(row.Amount, 2),
                Quantity = row.Quantity
            }).ToList();
        }

        private static async Task<List<SaleChartPointModel>> PaymentReport(IQueryable<domain.Operation.Sale> sales)
        {
            var cashAmount = await sales.Where(sale => sale.SaleType == SaleType.Cash).SumAsync(sale => (double?)sale.Amount) ?? 0;
            var cashCount = await sales.CountAsync(sale => sale.SaleType == SaleType.Cash);
            var creditAmount = await sales.Where(sale => sale.SaleType == SaleType.Credit).SumAsync(sale => (double?)sale.Amount) ?? 0;
            var creditCount = await sales.CountAsync(sale => sale.SaleType == SaleType.Credit);
            var uninvoiced = await sales
                .Where(sale => sale.SaleType == SaleType.Credit && sale.SaleInvoiceId == null)
                .SumAsync(sale => (double?)sale.Amount) ?? 0;

            var invoiceRows = await sales
                .Where(sale => sale.SaleType == SaleType.Credit && sale.SaleInvoiceId != null)
                .Select(sale => new { sale.SaleInvoiceId, sale.SaleInvoice.Balance })
                .ToListAsync();
            var pending = uninvoiced + invoiceRows
                .GroupBy(row => row.SaleInvoiceId)
                .Sum(group => group.First().Balance);

            return new List<SaleChartPointModel>
            {
                new SaleChartPointModel { Label = "Contado", Series = "Tipo", Amount = Math.Round(cashAmount, 2), Quantity = cashCount },
                new SaleChartPointModel { Label = "Crédito", Series = "Tipo", Amount = Math.Round(creditAmount, 2), Quantity = creditCount },
                new SaleChartPointModel { Label = "Pendiente de cobro", Series = "Pendiente", Amount = Math.Round(pending, 2), Quantity = creditCount }
            };
        }

    }
}
