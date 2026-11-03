using cod.domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.domain.Operation
{
    [Table("InvoicePayments", Schema = "operation")]
    public class InvoicePayment : Entity
    {
        public int InvoiceId { get; set; }
        [ForeignKey("InvoiceId")]
        public virtual ExpenseInvoice Invoice { get; set; }
        public DateTime PaymentDate { get; set; }
        public double Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
