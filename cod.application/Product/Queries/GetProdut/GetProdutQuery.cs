using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Product.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Product.Queries.GetProdut
{
    public class GetProdutQuery : BaseQuery, IGetProdutQuery
    {
        public GetProdutQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<ProductModel> Execute(int id)
        {
            var baseQuery = _repository.Query<domain.Data.Product>().Where(f => f.Id == id).Select(f => new
            {
                f.Id,
                f.Name,
                f.ProductType
            });

            var product = await baseQuery.FirstOrDefaultAsync();

            if (product == null)
                throw new ApplicationException("Producto no existe", "PRODUCTO");

            return new ProductModel
            {
                Id = product.Id,
                Name = product.Name,
                ProductType = product.ProductType,
            };
        }
    }
}
