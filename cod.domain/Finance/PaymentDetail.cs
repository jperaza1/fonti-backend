using cod.domain.Common;
using cod.domain.Operation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Finance
{
    [Table("PaymentDetails", Schema = "finance")]
    public class PaymentDetail : Entity
    {
        public int PaymentId { get; set; }
        [ForeignKey("PaymentId")]
        public virtual Payment Payment { get; set; }
        public int InvoiceId { get; set; }
        [ForeignKey("InvoiceId")]
        public virtual ExpenseInvoice Invoice { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
    }
}
