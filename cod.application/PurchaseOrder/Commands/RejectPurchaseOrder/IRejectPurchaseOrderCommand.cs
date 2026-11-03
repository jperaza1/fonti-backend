using System.Threading.Tasks;

namespace cod.application.PurchaseOrder.Commands.RejectPurchaseOrder
{
    public interface IRejectPurchaseOrderCommand
    {
        Task Execute(RejectPurchaseOrderModel model);
    }
}
