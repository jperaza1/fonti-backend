using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Product.Common;
using cod.domain.Features.Enums;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Licensing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Product.Queries.GetActiveProductsByType
{
    public class GetActiveProductsByTypeQuery : BaseQuery, IGetActiveProductsByTypeQuery
    {
        public GetActiveProductsByTypeQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<ProductModel>> Execute(ProductType productType)
        {
            var baseQuery = _repository.QueryActive<domain.Data.Product>()
                .Where(f => f.ProductType == productType)
                .Select(f => new
            {
                f.Id,
                f.Name,
                f.ProductType,
            });

            return await baseQuery.Select(f => new ProductModel
            {
                Id = f.Id,
                Name = f.Name,
                ProductType = f.ProductType,
                ProductTypeName = f.ProductType.GetEnumDescription()
            }).ToListAsync();
        }
    }
}
