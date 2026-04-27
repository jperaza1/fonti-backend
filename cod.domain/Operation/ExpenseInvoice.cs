using cod.domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Operation
{
    [Table("ExpenseInvoices", Schema = "operation")]
    public class ExpenseInvoice : Entity
    {
        public int ExpenseId { get; set; }
        [ForeignKey("ExpenseId")]
        public virtual Expense Expense { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public double TaxableAmount { get; set; }
        public double ExemptAmount { get; set; }
        public double Discount { get; set; }
        public double SubTotal { get; set; }
        public double TaxAmount { get; set; }
        public double Total { get; set; }
        public bool Paid { get; set; }
        public DateTime? PaidAt { get; set; }
        public string PaidUser { get; set; }
    }
}
