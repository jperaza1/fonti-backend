using cod.application.Warehouse.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.Warehouse.Queries.GetWarehouses
{
    public interface IGetWarehousesQuery
    {
        Task<List<WarehouseModel>> Execute();
    }
}
