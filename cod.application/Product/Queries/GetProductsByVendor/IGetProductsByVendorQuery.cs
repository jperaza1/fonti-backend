using cod.application.Product.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Product.Queries.GetProductsByVendor
{
    public interface IGetProductsByVendorQuery
    {
        Task<List<ProductModel>> Execute(int vendorId);
    }
}
