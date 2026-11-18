using cod.application.WarehouseTransfer.Common;
using System.Threading.Tasks;

namespace cod.application.WarehouseTransfer.Queries.GetWarehouseTransfer
{
    public interface IGetWarehouseTransferQuery
    {
        Task<WarehouseTransferModel> Execute(int id);
    }
}
