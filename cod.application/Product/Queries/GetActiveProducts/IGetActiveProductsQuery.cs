using cod.application.Product.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Product.Queries.GetActiveProducts
{
    public interface IGetActiveProductsQuery
    {
        Task<List<ProductModel>> Execute();
    }
}
