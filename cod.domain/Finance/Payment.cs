using cod.domain.Common;
using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Finance
{
    [Table("Payments", Schema = "finance")]
    public class Payment : Entity
    {
        public string Reference { get; set; }
        public int BankAccountId { get; set; }
        [ForeignKey("BankAccountId")]
        public virtual BankAccount BankAccount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
        public double Amount { get; set; }
        public double InvoiceCount { get; set; }
        public string BankReference { get; set; }
        public string Notes { get; set; }
        public virtual ICollection<PaymentDetail> Details { get; set; }
    }
}
