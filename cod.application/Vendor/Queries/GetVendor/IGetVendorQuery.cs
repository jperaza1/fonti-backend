using cod.application.Vendor.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Vendor.Queries.GetVendor
{
    public interface IGetVendorQuery
    {
        Task<VendorModel> Execute(int id);
    }
}
