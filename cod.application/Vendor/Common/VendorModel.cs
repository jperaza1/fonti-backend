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
        public int? BankId { get; set; }
        public string Bank { get; set; }
        public string BankAccount { get; set; }
        public string BankAccountHolder { get; set; }
        public string IdentificationHolder { get; set; }
        public string C1 { get; set; }
        public string C2 { get; set; }
        public int? TypeAccountId { get; set; }
        public string TypeAccount { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
