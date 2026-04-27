using cod.application.PurchaseOrder.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.PurchaseOrder.Commands.CreatePurchaseOrder
{
    public interface ICreatePurchaseOrderCommand
    {
        Task<int> Execute(PurchaseOrderModel model);
    }
}
