using cod.domain.Common;
using cod.domain.Finance;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.domain.Operation
{
    [Table("ExpenseInvoices", Schema = "operation")]
    public class ExpenseInvoice : Entity
    {
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
        public double Balance { get; set; }
        public DateTime? DueDate { get; set; }
        public PaymentType PaymentType { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public bool IsRefund { get; set; }
        public int? DocumentId { get; set; }
        public string FileUploadedUser { get; set; }
        public DateTime? FileUploadedAt { get; set; }
        public virtual Expense Expense { get; set; }
        public int? PaymentId { get; set; }
        [ForeignKey("PaymentId")]
        public virtual Payment Payment { get; set; }
        public virtual ICollection<InvoicePayment> Payments { get; set; }
    }
}
