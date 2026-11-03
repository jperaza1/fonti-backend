using System.Threading.Tasks;

namespace cod.application.PurchaseOrder.Commands.UpdateStatus
{
    public interface IUpdateStatusCommand
    {
        Task Execute(int id);
    }
}
