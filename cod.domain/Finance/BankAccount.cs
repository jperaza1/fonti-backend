using cod.domain.Common;
using cod.domain.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Finance
{
    [Table("BankAccounts", Schema = "finance")]
    public class BankAccount : Entity
    {
        public int BankId { get; set; }
        [ForeignKey("BankId")]
        public virtual Bank Bank { get; set; }
        public string Name { get; set; }
        public double Balance { get; set; } 
    }
}
