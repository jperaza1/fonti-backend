using cod.application.WarehouseTransfer.Common;
using System.Threading.Tasks;

namespace cod.application.WarehouseTransfer.Commands.RejectWarehouseTransfer
{
    public interface IRejectWarehouseTransferCommand
    {
        Task Execute(RejectWarehouseTransferModel model);
    }
}
