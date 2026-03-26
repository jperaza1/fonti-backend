using cod.application.Product.Common;
using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Product.Queries.GetActiveProductsByType
{
    public interface IGetActiveProductsByTypeQuery
    {
        Task<List<ProductModel>> Execute(ProductType type);
    }
}
