using System;
using System.Collections.Generic;

namespace cod.application.InvoiceSale.Queries.GetInvoiceReceiptPdf
{
    public class InvoiceReceiptModel
    {
        public string Logo { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CompanyRtn { get; set; }

        public string Cai { get; set; }
        public DateTime EmissionLimitDate { get; set; }
        public string CorrelativeFrom { get; set; }
        public string CorrelativeTo { get; set; }

        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string PaymentTypeName { get; set; }
        public DateTime? DueDate { get; set; }

        public string CustomerName { get; set; }
        public string CustomerRtn { get; set; }

        public string Advisor { get; set; }
        public string ExemptCertificateNumber { get; set; }
        public string ExemptOrderNumber { get; set; }
        public string SagRegistryNumber { get; set; }

        public double TaxableAmount { get; set; }
        public double TaxableAmount18 { get; set; }
        public double ExemptAmount { get; set; }
        public double ExoneratedAmount { get; set; }
        public double Discount { get; set; }
        public double SubTotal { get; set; }
        public double TaxAmount { get; set; }
        public double Tax18Amount { get; set; }
        public double Total { get; set; }

        public List<InvoiceReceiptLineModel> Lines { get; set; } = new List<InvoiceReceiptLineModel>();
    }

    public class InvoiceReceiptLineModel
    {
        public double Quantity { get; set; }
        public string Description { get; set; }
        public bool Tax { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
    }
}
