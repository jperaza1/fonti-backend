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
        public int? BankId { get; set; }
        [ForeignKey("BankId")]
        public string Email { get; set; }
        public virtual Bank Bank { get; set; }
        public string BankAccount { get; set; }
        public string BankAccountHolder { get; set; }
        public string IdentificationHolder { get; set; }
        public int? TypeAccountId { get; set; }
        [ForeignKey("TypeAccountId")]
        public virtual Type TypeAccount { get; set; }
        public string C1 { get; set; }
        public string C2 { get; set; }
    }
}
