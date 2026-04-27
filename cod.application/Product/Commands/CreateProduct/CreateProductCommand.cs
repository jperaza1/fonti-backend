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
                ProductType = model.ProductType,
            };

            _repository.InsertAsync(newProduct);

            await _repository.CommitAsync();
        }

        public async Task ValidateModel(ProductModel model)
        {
            if (model.Name.NullOrEmpty())
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Required));

            if (!model.ProductType.IsValid())
                this._exceptionFactory.Add("productType", this._exceptionFactory.GetMessage("Tipo de Producto", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "PRODUCTO", this._exceptionFactory.Exceptions);

            bool duplicateName = await _repository
                .AnyAsync<domain.Data.Product>(f => f.Name == model.Name && f.ProductType != model.ProductType);

            if (duplicateName)
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Duplicate));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "PRODUCTO", this._exceptionFactory.Exceptions);
        }
    }
}
