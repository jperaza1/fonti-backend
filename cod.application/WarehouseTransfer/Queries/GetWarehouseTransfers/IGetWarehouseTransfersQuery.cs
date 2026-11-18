using cod.application.WarehouseTransfer.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.WarehouseTransfer.Queries.GetWarehouseTransfers
{
    public interface IGetWarehouseTransfersQuery
    {
        Task<List<WarehouseTransferModel>> Execute();
    }
}
