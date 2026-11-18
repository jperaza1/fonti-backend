using System.Threading.Tasks;

namespace cod.application.WarehouseTransfer.Commands.AcceptWarehouseTransfer
{
    public interface IAcceptWarehouseTransferCommand
    {
        Task Execute(int id);
    }
}
