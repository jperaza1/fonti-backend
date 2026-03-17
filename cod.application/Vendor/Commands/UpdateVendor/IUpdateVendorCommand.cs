using cod.application.Vendor.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Vendor.Commands.UpdateVendor
{
    public interface IUpdateVendorCommand
    {
        Task Execute(VendorModel model, int vendorId);
    }
}
