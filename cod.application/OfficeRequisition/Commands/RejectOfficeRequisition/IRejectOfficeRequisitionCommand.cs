using cod.application.PurchaseOrder.Commands.RejectPurchaseOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.OfficeRequisition.Commands.RejectOfficeRequisition
{
    public interface IRejectOfficeRequisitionCommand
    {
        Task Execute(RejectPurchaseOrderModel model);
    }
}
