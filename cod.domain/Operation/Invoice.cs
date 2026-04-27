using cod.domain.Common;
using cod.domain.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.domain.Operation
{
    [Table("Invoices", Schema = "operation")]
    public class Invoice : Entity
    {
        public int? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        public string InvoiceNumber { get; set; }
        public string Description { get; set; }
        public DateTime InvoiceDate { get; set; }
        public double TaxableAmount { get; set; }
        public double ExemptAmount { get; set; }
        public double Discount { get; set; }
        public double SubTotal { get; set; }
        public double TaxAmount { get; set; }
        public double Total { get; set; }
        public double Balance { get; set; }
        public DateTime? DueDate { get; set; }
        public bool Paid { get; set; }
        public DateTime? PaidAt { get; set; }
        public string PaidUser { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
