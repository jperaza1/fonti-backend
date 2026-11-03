using cod.application.Product.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.Product.Queries.GetProductsByVendor
{
    public interface IGetProductsByVendorQuery
    {
        Task<List<VendorProductModel>> Execute(int vendorId);
    }
}
