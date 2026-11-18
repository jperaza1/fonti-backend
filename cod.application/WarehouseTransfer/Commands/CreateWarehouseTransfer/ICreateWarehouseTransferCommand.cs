using cod.application.WarehouseTransfer.Common;
using System.Threading.Tasks;

namespace cod.application.WarehouseTransfer.Commands.CreateWarehouseTransfer
{
    public interface ICreateWarehouseTransferCommand
    {
        Task<int> Execute(WarehouseTransferModel model);
    }
}
