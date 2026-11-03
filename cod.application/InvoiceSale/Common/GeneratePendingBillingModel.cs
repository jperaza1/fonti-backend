using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.InvoiceSale.Common
{
    public class GeneratePendingBillingModel
    {
        public SaleType SaleType { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public List<int> SaleIds { get; set; }
        public List<GeneratePendingBillingCustomerModel> Customers { get; set; }
    }

    public class GeneratePendingBillingCustomerModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime InvoiceDate { get; set; }
        public List<int> SaleIds { get; set; }
    }
}
