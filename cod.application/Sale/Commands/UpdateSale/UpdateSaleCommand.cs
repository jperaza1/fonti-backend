using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Sale.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Sale.Commands.UpdateSale
{
    public class UpdateSaleCommand : BaseCommand, IUpdateSaleCommand
    {
        public UpdateSaleCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(SaleModel model, int id)
        {
            ValidateModel(model);

            var sale = await _repository.Query<domain.Operation.Sale>().FirstOrDefaultAsync(f => f.Id == id);

            if (sale == null)
                throw new ApplicationException("Venta no existe", "VENTAS");

            sale.CafeteriaId = model.CafeteriaId;
            sale.CategoryId = model.CategoryId;
            sale.Amount = model.Amount;
            sale.SaleDate = model.SaleDate;
            sale.SaleType = model.SaleType;
            sale.ProductTypeId = model.ProductTypeId;
            sale.RegisterId = model.RegisterId;
            sale.ServicePeriodId = model.ServicePeriodId;
            sale.CustomerId = model.CustomerId;

            _repository.UpdateSync(sale);

            await _repository.CommitAsync();
        }

        public void ValidateModel(SaleModel model)
        {
            if (model.CafeteriaId <= 0)
                this._exceptionFactory.Add("cafeteria", this._exceptionFactory.GetMessage("Cafeteria", ExceptionEnums.ExceptionCode.Required));

            if (!model.SaleType.IsValid())
                this._exceptionFactory.Add("saleType", this._exceptionFactory.GetMessage("Tipo de Venta", ExceptionEnums.ExceptionCode.Required));

            if (model.CategoryId <= 0)
                this._exceptionFactory.Add("category", this._exceptionFactory.GetMessage("Categoria", ExceptionEnums.ExceptionCode.Required));

            if (model.ProductTypeId <= 0)
                this._exceptionFactory.Add("product", this._exceptionFactory.GetMessage("Producto", ExceptionEnums.ExceptionCode.Required));

            if (model.Amount <= 0)
                this._exceptionFactory.Add("amount", this._exceptionFactory.GetMessage("Monto", ExceptionEnums.ExceptionCode.Required));

            if (!model.RegisterId.HasValue && model.RegisterId.Value <= 0)
                this._exceptionFactory.Add("register", this._exceptionFactory.GetMessage("Caja", ExceptionEnums.ExceptionCode.Required));

            if (!model.CustomerId.HasValue || model.CustomerId.Value <= 0)
                this._exceptionFactory.Add("customer", this._exceptionFactory.GetMessage("Cliente", ExceptionEnums.ExceptionCode.Required));

            if (!model.ServicePeriodId.HasValue && model.ServicePeriodId.Value <= 0)
                this._exceptionFactory.Add("servicePeriod", this._exceptionFactory.GetMessage("Período de servicio", ExceptionEnums.ExceptionCode.Required));

            if (model.SaleDate == DateTime.MinValue)
                this._exceptionFactory.Add("saleDate", this._exceptionFactory.GetMessage("Fecha de Venta", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "VENTAS", this._exceptionFactory.Exceptions);


        }
    }
}
