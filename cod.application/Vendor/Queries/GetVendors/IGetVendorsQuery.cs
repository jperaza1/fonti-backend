using cod.application.Vendor.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Vendor.Queries.GetVendors
{
    public interface IGetVendorsQuery
    {
        Task<List<VendorModel>> Execute();
    }
}
