using cod.application.Common.Clases;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Sale.Common;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.Sale.Commands.UpdateSale
{
    public class UpdateSaleCommand : BaseCommand, IUpdateSaleCommand
    {
        private readonly AuthService _authService;

        public UpdateSaleCommand(IRepository repository, IExceptionFactory exceptionFactory, AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task Execute(SaleModel model, int id)
        {
            var timeZone = _authService.ClientTimezone;
            model.SaleDate = DateService.GetCLientLocalTime(model.SaleDate, timeZone);
            ValidateModel(model);

            var sale = await _repository.Query<domain.Operation.Sale>().FirstOrDefaultAsync(f => f.Id == id);
            if (sale == null)
                throw new ApplicationException("Venta no existe", "VENTAS");

            if (sale.SaleInvoiceId.HasValue)
                throw new ApplicationException("La venta ya tiene factura y no se puede editar", "VENTAS");

            var currentDetails = await _repository.Query<domain.Operation.SaleDetail>()
                .Where(f => f.SaleId == id)
                .ToListAsync();

            var incoming = (model.SaleDetails ?? new System.Collections.Generic.List<SaleDetailModel>())
                .Where(f => f.Quantity > 0)
                .ToList();

            var incomingIds = incoming.Where(f => f.Id > 0).Select(f => f.Id).ToList();
            foreach (var detail in currentDetails.Where(f => !incomingIds.Contains(f.Id)))
                _repository.DeleteSync(detail);

            foreach (var item in incoming.Where(f => f.Id > 0))
            {
                var detail = currentDetails.FirstOrDefault(f => f.Id == item.Id);
                if (detail == null)
                    continue;

                detail.ProductId = item.ProductId;
                detail.Price = item.Price;
                detail.Quantity = item.Quantity;
                detail.Total = item.Total;
                _repository.UpdateSync(detail);
            }

            foreach (var item in incoming.Where(f => f.Id <= 0))
            {
                _repository.InsertAsync(new domain.Operation.SaleDetail
                {
                    SaleId = id,
                    ProductId = item.ProductId,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Total = item.Total,
                    IsActive = true,
                    CreationTime = Clock.Now,
                    CreatorUser = _authService.User
                });
            }

            sale.RouteId = model.RouteId;
            sale.Amount = model.Amount;
            sale.SaleDate = model.SaleDate;
            sale.SaleType = model.SaleType;
            sale.CustomerId = model.SaleType == SaleType.Credit ? model.CustomerId : null;

            _repository.UpdateSync(sale);
            await _repository.CommitAsync();
        }

        private void ValidateModel(SaleModel model)
        {
            if (model.RouteId <= 0)
                this._exceptionFactory.Add("route", this._exceptionFactory.GetMessage("Ruta", ExceptionEnums.ExceptionCode.Required));

            if (!model.SaleType.IsValid())
                this._exceptionFactory.Add("saleType", this._exceptionFactory.GetMessage("Tipo de Venta", ExceptionEnums.ExceptionCode.Required));

            var regaliaWithQuantity = model.SaleDetails != null &&
                model.SaleDetails.Any(detail => detail.Quantity > 0 && detail.Price == 0);

            if (model.Amount < 0 || (model.Amount <= 0 && !regaliaWithQuantity))
                this._exceptionFactory.Add("amount", this._exceptionFactory.GetMessage("Monto", ExceptionEnums.ExceptionCode.Required));

            if (model.SaleDate == DateTime.MinValue)
                this._exceptionFactory.Add("saleDate", this._exceptionFactory.GetMessage("Fecha de Venta", ExceptionEnums.ExceptionCode.Required));

            if (model.SaleType == SaleType.Credit && (!model.CustomerId.HasValue || model.CustomerId.Value <= 0))
                this._exceptionFactory.Add("customerId", this._exceptionFactory.GetMessage("Cliente", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "VENTAS", this._exceptionFactory.Exceptions);
        }
    }
}
