using cod.application.PurchaseOrder.Commands.RejectPurchaseOrder;
using System.Threading.Tasks;

namespace cod.application.PurchaseOrder.Commands.CancelPurchaseOrder
{
    public interface ICancelPurchaseOrderCommand
    {
        Task Execute(RejectPurchaseOrderModel model);
    }
}
