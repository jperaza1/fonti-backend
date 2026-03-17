using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Vendor.Commands.EnableVendor
{
    public interface IEnableVendorCommand
    {
        Task Execute(int id);
    }
}
