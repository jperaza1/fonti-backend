using System;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.PurchaseOrder.Queries.GetPurchasesOrder
{
    public class FilterPurchaseOrder
    {
        public PurchaseOrderStatus OrderStatus { get; set; }
        public int VendorId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
