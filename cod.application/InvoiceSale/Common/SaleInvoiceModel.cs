using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.InvoiceSale.Common
{
    public class SaleInvoiceModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Customer { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public double TaxableAmount { get; set; }
        public double TaxableAmount18 { get; set; }
        public double ExemptAmount { get; set; }
        public double Discount { get; set; }
        public double SubTotal { get; set; }
        public double TaxAmount { get; set; }
        public double Tax18Amount { get; set; }
        public double Total { get; set; }
        public double Balance { get; set; }
        public DateTime? DueDate { get; set; }
        public bool Paid { get; set; }
        public DateTime? PaidAt { get; set; }
        public string PaidUser { get; set; }
        public PaymentType PaymentType { get; set; }
        public string PaymentTypeName { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentStatusName { get; set; }
        public DateTime CreationTime { get; set; }
        public string CreatorUser { get; set; }
        public List<SaleInvoiceDetailModel> Details { get; set; }
    }

    public class SaleInvoiceDetailModel
    {
        public int Id { get; set; }
        public int SaleInvoiceId { get; set; }
        public int SaleId { get; set; }
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public string SaleTypeName { get; set; }
        public DateTime SaleDate { get; set; }
        public double Amount { get; set; }
        public bool IsActive { get; set; }
    }
}
