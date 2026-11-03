using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Payment.Common
{
    public class PaymentReceiptModel
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public string BankName { get; set; }
        public string BankReference { get; set; }
        public DateTime Date { get; set; }
        public string Cafeteria { get; set; }
        public string Vendor { get; set; }
        public string Address { get; set; }
        public string Rtn { get; set; }
        public string Phone { get; set; }
        public string BankAccount { get; set; }
        public string DestinationOffice { get; set; }
        public string DestinationBranch { get; set; }
        public double Total { get; set; }
        public List<PaymentReceiptDetailModel> Details { get; set; }
    }

    public class PaymentReceiptDetailModel
    {
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public double Total { get; set; }
    }
}
