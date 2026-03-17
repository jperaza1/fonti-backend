using cod.application.Product.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Product.Queries.GetProducts
{
    public interface IGetProductsQuery
    {
        Task<List<ProductModel>> Execute();
    }
}
