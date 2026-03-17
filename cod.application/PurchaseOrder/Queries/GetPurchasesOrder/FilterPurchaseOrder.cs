using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.PurchaseOrder.Queries.GetPurchasesOrder
{
    public class FilterPurchaseOrder
    {
        public DomainEnums.UserType UserType { get; set; }
        public List<int> Cafeterias { get; set; }
        public int? CafeteriaId { get; set; }
        public PurchaseOrderStatus OrderStatus { get; set; }
        public int VendorId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}
