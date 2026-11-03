using cod.application.ExpenseInvoice.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.Expense.Common
{
    public class ExpenseModel
    {
        public int Id { get; set; }
        public int? TruckId { get; set; }
        public string TruckName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public int? VendorId { get; set; }
        public string VendorName { get; set; }
        public int RouteId { get; set; }
        public string Route { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        public bool IsInvoice { get; set; }
        public bool IsRefund { get; set; }
        public PaymentType PaymentType { get; set; }
        public string InvoiceNumber { get; set; }
        public int? InvoiceId { get; set; }
        public double Total { get; set; }
        public bool IsActive { get; set; }
        public string DocumentUploadedUser { get; set; }
        public DateTime? DocumentUploadedAt { get; set; }
        public int? DocumentId { get; set; }
        public ExpenseInvoiceModel ExpenseInvoice { get; set; }
    }
}
