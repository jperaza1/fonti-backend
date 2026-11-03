using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Payment.Common
{
    public class PaymentModel
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public int BankAccountId { get; set; }
        public string BankAccount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentStatusName { get; set; }
        public DateTime PaymentDate { get; set; }
        public double Amount { get; set; }
        public double InvoiceCount { get; set; }
        public string BankReference { get; set; }
        public string Notes { get; set; }
        public List<PaymentDetailModel> Details { get; set; }
    }

    public class PaymentDetailModel
    {
        public int Id { get; set; }
        public int PaymentId { get; set; }
        public int InvoiceId { get; set; }
        public int VendorId { get; set; }
        public double Amount { get; set; }
        public double RemainingDays { get; set; }
        public string VendorName { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string Description { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string InvoiceNote { get; set; }
        public string DestinationBranch { get; set; }
        public string DestinationOffice { get; set; }
        public string BankAccountHolder { get; set; }
    }
}
