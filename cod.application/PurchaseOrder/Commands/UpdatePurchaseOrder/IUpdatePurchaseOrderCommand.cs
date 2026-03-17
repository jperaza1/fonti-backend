using cod.application.PurchaseOrder.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.PurchaseOrder.Commands.UpdatePurchaseOrder
{
    public interface IUpdatePurchaseOrderCommand
    {
        Task Execute(PurchaseOrderModel model, int id);
    }
}
