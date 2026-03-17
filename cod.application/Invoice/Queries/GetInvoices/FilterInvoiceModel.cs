using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.Invoice.Queries.GetInvoices
{
    public class FilterInvoiceModel
    {
        public InvoiceType InvoiceType { get; set; }
        public PaymentType PaymentType { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int CafeteriaId { get; set; }
        public int VendorId { get; set; }
    }
}
