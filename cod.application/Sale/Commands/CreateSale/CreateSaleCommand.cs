using cod.application.Common.Clases;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Route.Common;
using cod.application.Sale.Common;
using cod.domain.Features;
using cod.domain.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Sale.Commands.CreateSale
{
    public class CreateSaleCommand : BaseCommand, ICreateSaleCommand
    {
        private readonly AuthService _authService;
        public CreateSaleCommand(IRepository repository, IExceptionFactory exceptionFactory, AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task Execute(SaleModel model)
        {
            var timeZone = _authService.ClientTimezone;
            model.SaleDate = DateService.GetCLientLocalTime(model.SaleDate, timeZone);
            ValidateModel(model);

            var newSale = new domain.Operation.Sale
            {
                RouteId = model.RouteId,
                Amount = model.Amount,
                SaleDate = model.SaleDate,
                SaleType = model.SaleType,
                CustomerId = model.CustomerId,
                SaleDetails = GenerateDetails(model)
            };

            _repository.InsertAsync(newSale);

            await _repository.CommitAsync();
        }

        public List<domain.Operation.SaleDetail> GenerateDetails(SaleModel model)
        {
            var lst = new List<domain.Operation.SaleDetail>();
            foreach (var item in model.SaleDetails.Where(f => f.Quantity > 0).ToList())
            {
                var detail = new domain.Operation.SaleDetail
                {
                    ProductId = item.ProductId,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Total = item.Total,
                    IsActive = true,
                    CreationTime = Clock.Now,
                    CreatorUser = _authService.User,
                };
                lst.Add(detail);
            }
            return lst;
        }

        public void ValidateModel(SaleModel model) 
        {
            if(model.RouteId <= 0)
                this._exceptionFactory.Add("route", this._exceptionFactory.GetMessage("Ruta", ExceptionEnums.ExceptionCode.Required));

            if(!model.SaleType.IsValid())
                this._exceptionFactory.Add("saleType", this._exceptionFactory.GetMessage("Tipo de Venta", ExceptionEnums.ExceptionCode.Required));

            var regaliaWithQuantity = model.SaleDetails != null &&
                model.SaleDetails.Any(detail => detail.Quantity > 0 && detail.Price == 0);

            if (model.Amount < 0 || (model.Amount <= 0 && !regaliaWithQuantity))
                this._exceptionFactory.Add("amount", this._exceptionFactory.GetMessage("Monto", ExceptionEnums.ExceptionCode.Required));

            if (model.SaleDate == DateTime.MinValue)
                this._exceptionFactory.Add("saleDate", this._exceptionFactory.GetMessage("Fecha de Venta", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "VENTAS", this._exceptionFactory.Exceptions);

            if(model.SaleType == domain.Features.Enums.DomainEnums.SaleType.Credit && (!model.CustomerId.HasValue || model.CustomerId.Value <= 0))
                this._exceptionFactory.Add("customerId", this._exceptionFactory.GetMessage("Cliente", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "VENTAS", this._exceptionFactory.Exceptions);
        }    
    }
}
