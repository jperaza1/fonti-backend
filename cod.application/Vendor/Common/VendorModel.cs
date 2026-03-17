using cod.application.Main.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Vendor.Common
{
    public class VendorModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Rtn { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Seller { get; set; }
        public bool HasCredit { get; set; }
        public int CreditDays { get; set; }
        public bool IsActive { get; set; }
        public bool HasInvoice { get; set; }
        public bool RequiresAdminApproval { get; set; }
    }
}
