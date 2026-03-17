using cod.domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Data
{
    [Table("Vendors", Schema = "data")]
    public class Vendor : Entity
    {
        public string Name { get; set; }
        public string Rtn { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Seller { get; set; }
        public bool HasCredit { get; set; }
        public bool HasInvoice { get; set; }
        public bool RequiresAdminApproval { get; set; }
        public int CreditDays { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
