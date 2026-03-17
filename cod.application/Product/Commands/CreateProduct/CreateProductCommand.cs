using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Product.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Product.Commands.CreateProduct
{
    public class CreateProductCommand : BaseCommand, ICreateProductCommand
    {
        public CreateProductCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(ProductModel model)
        {
            await ValidateModel(model);

            var newProduct = new domain.Data.Product
            {
                Name = model.Name,
                VendorId = model.VendorId,
                MeasureId = model.MeasureId,
                Cost = model.Cost,
                Unit = model.Unit,
                Tax = model.Tax,
                Discount = model.Discount,
                UnitCost = model.UnitCost,
                SpecialServiceUse = model.SpecialServiceUse,
            };

            _repository.InsertAsync(newProduct);

            await _repository.CommitAsync();
        }

        public async Task ValidateModel(ProductModel model)
        {
            if (model.Name.NullOrEmpty())
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Required));

            if (model.VendorId <= 0)
                this._exceptionFactory.Add("vendor", this._exceptionFactory.GetMessage("Proveedor", ExceptionEnums.ExceptionCode.Required));

            if (model.MeasureId <= 0)
                this._exceptionFactory.Add("measure", this._exceptionFactory.GetMessage("Medida", ExceptionEnums.ExceptionCode.Required));

            if (model.Unit <= 0)
                this._exceptionFactory.Add("unit", this._exceptionFactory.GetMessage("Unidades por medida", ExceptionEnums.ExceptionCode.Required));

            if (model.Cost <= 0)
                this._exceptionFactory.Add("cost", this._exceptionFactory.GetMessage("Costo", ExceptionEnums.ExceptionCode.Required));

            if (model.UnitCost <= 0)
                this._exceptionFactory.Add("unitCost", this._exceptionFactory.GetMessage("Costo Unitario", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "PRODUCTO", this._exceptionFactory.Exceptions);

            bool duplicateName = await _repository
                .AnyAsync<domain.Data.Product>(f => f.Name == model.Name && f.VendorId != model.VendorId && f.MeasureId != model.MeasureId);

            if (duplicateName)
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Duplicate));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "PRODUCTO", this._exceptionFactory.Exceptions);
        }
    }
}
