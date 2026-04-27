using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Main;
using cod.application.Product.Common;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Interfaces.Drawing.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Product.Queries.GetProducts
{
    public class GetProductsQuery : BaseQuery, IGetProductsQuery
    {
        public GetProductsQuery(
            IRepository repository
            ) : base(repository)
        {
            
        }

        public async Task<List<ProductModel>> Execute()
        {
            var baseQuery = _repository.Query<domain.Data.Product>().Select(f => new
            {
                f.Id,
                f.Name,
                f.ProductType,
                f.IsActive
            });

            return await baseQuery.Select(f => new ProductModel
            {
                Id = f.Id,
                Name = f.Name,
                ProductType = f.ProductType,
                ProductTypeName = f.ProductType.GetEnumDescription(),
                IsActive = f.IsActive
            }).ToListAsync();
        }
    }
}
