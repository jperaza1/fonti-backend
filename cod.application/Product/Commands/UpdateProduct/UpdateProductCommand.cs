using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Product.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Product.Commands.UpdateProduct
{
    public class UpdateProductCommand : BaseCommand, IUpdateProductCommand
    {
        public UpdateProductCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(ProductModel model, int id)
        {
            ValidateModel(model);

            var product = await _repository.Query<domain.Data.Product>().Where(f => f.Id == id).FirstOrDefaultAsync();

            if (product == null) throw new ApplicationException("Producto no existe", "PRODUCTO", this._exceptionFactory.Exceptions);

            product.Name = model.Name;
            product.ProductType = model.ProductType;

            _repository.UpdateSync(product);

            await _repository.CommitAsync();
        }

        public void ValidateModel(ProductModel model)
        {
            if (model.Name.NullOrEmpty())
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Required));

            if (model.ProductType.IsValid())
                this._exceptionFactory.Add("productType", this._exceptionFactory.GetMessage("Tipo de Producto", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "PRODUCTO", this._exceptionFactory.Exceptions);
        }
    }
}
