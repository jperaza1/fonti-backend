using cod.application.Common.Clases;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
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
                CafeteriaId = model.CafeteriaId,
                CategoryId = model.CategoryId,
                CustomerId = model.CustomerId,
                Amount = model.Amount,
                ProductTypeId = model.ProductTypeId,
                SaleDate = model.SaleDate,
                SaleType = model.SaleType,
                RegisterId = model.RegisterId,
                ServicePeriodId = model.ServicePeriodId,
            };

            _repository.InsertAsync(newSale);

            await _repository.CommitAsync();
        }

        public void ValidateModel(SaleModel model) 
        {
            if(model.CafeteriaId <= 0)
                this._exceptionFactory.Add("cafeteria", this._exceptionFactory.GetMessage("Cafeteria", ExceptionEnums.ExceptionCode.Required));

            if(!model.SaleType.IsValid())
                this._exceptionFactory.Add("saleType", this._exceptionFactory.GetMessage("Tipo de Venta", ExceptionEnums.ExceptionCode.Required));

            if(model.CategoryId <= 0)
                this._exceptionFactory.Add("category", this._exceptionFactory.GetMessage("Categoria", ExceptionEnums.ExceptionCode.Required));

            if (model.ProductTypeId <= 0)
                this._exceptionFactory.Add("product", this._exceptionFactory.GetMessage("Producto", ExceptionEnums.ExceptionCode.Required));

            if (model.Amount <= 0)
                this._exceptionFactory.Add("amount", this._exceptionFactory.GetMessage("Monto", ExceptionEnums.ExceptionCode.Required));

            if(!model.RegisterId.HasValue || model.RegisterId.Value <= 0)
                this._exceptionFactory.Add("register", this._exceptionFactory.GetMessage("Caja", ExceptionEnums.ExceptionCode.Required));

            if (!model.ServicePeriodId.HasValue || model.ServicePeriodId.Value <= 0)
                this._exceptionFactory.Add("servicePeriod", this._exceptionFactory.GetMessage("Período de servicio", ExceptionEnums.ExceptionCode.Required));

            if (!model.CustomerId.HasValue || model.CustomerId.Value <= 0)
                this._exceptionFactory.Add("customer", this._exceptionFactory.GetMessage("Cliente", ExceptionEnums.ExceptionCode.Required));

            if (model.SaleDate == DateTime.MinValue)
                this._exceptionFactory.Add("saleDate", this._exceptionFactory.GetMessage("Fecha de Venta", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "VENTAS", this._exceptionFactory.Exceptions);


        }    
    }
}
