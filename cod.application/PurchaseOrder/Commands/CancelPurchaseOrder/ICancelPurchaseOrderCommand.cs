using cod.application.PurchaseOrder.Commands.RejectPurchaseOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.PurchaseOrder.Commands.CancelPurchaseOrder
{
    public interface ICancelPurchaseOrderCommand
    {
        Task Execute(RejectPurchaseOrderModel model);
    }
}
