using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.PurchaseOrder.Common
{
    public class PurchaseOrderInvoiceModel
    {
        public int Id { get; set; }
        public int? PurchaseOrderId { get; set; }
        public string VendorName { get; set; }
        public string PurchaseOrder { get; set; }
        public string InvoiceNumber { get; set; }
        public string Description { get; set; }
        public DateTime InvoiceDate { get; set; }
        public double TaxableAmount { get; set; }
        public double ExemptAmount { get; set; }
        public double Discount { get; set; }
        public double SubTotal { get; set; }
        public double TaxAmount { get; set; }
        public double Total { get; set; }
        public DateTime? DueDate { get; set; }
        public bool Paid { get; set; }
        public DateTime? PaidAt { get; set; }
        public string PaidUser { get; set; }
        public PaymentType PaymentType { get; set; }
        public string PaymentTypeName { get; set; }
    }
}
