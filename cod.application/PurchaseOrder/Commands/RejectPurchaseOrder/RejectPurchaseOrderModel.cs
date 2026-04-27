using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.PurchaseOrder.Commands.RejectPurchaseOrder
{
    public class RejectPurchaseOrderModel
    {
        public int PurchaseOrderId { get; set; }
        public int SpecialServiceId { get; set; }
        public int OfficeRequisitionId { get; set; } 
        public string Reason { get; set; }
    }
}
