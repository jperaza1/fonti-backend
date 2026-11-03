using cod.application.PurchaseOrder.Common;
using System.Threading.Tasks;

namespace cod.application.PurchaseOrder.Commands.CreatePurchaseOrder
{
    public interface ICreatePurchaseOrderCommand
    {
        Task<int> Execute(PurchaseOrderModel model);
    }
}
