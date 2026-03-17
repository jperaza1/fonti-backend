using cod.application.Payment.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.Invoice.Common
{
    public class InvoiceModel
    {
        public int Id { get; set; }
        public int? PurchaseOrderId { get; set; }
        public int CafeteriaId { get; set; }
        public string Cafeteria { get; set; }
        public int? VendorId { get; set; }
        public int? DocumentId { get; set; }
        public string Vendor { get; set; }
        public double RemainingDays { get; set; }
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
        public double Balance { get; set; }
        public DateTime? DueDate { get; set; }
        public bool Paid { get; set; }
        public DateTime? PaidAt { get; set; }
        public string PaidUser { get; set; }
        public PaymentType PaymentType { get; set; }
        public string PaymentTypeName { get; set; }
        public InvoiceType InvoiceType { get; set; }
        public string InvoiceTypeName { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentStatusName { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public List<PaymentModel> Payments { get; set; }
        public bool HasInvoice { get; set; }
        public bool HasFile { get; set; }
        public DateTime? FileUploadedAt { get; set; }
        public string FileUploadedUser { get; set; }
    }
}
