using cod.application.Common.Extensions;
using cod.application.Inventory.Services;
using cod.application.Warehouse.Common;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.Warehouse.Queries.GetWarehouses
{
    public class GetWarehousesQuery : IGetWarehousesQuery
    {
        private readonly IWarehouseDirectory _warehouses;

        public GetWarehousesQuery(IWarehouseDirectory warehouses)
        {
            _warehouses = warehouses;
        }

        public async Task<System.Collections.Generic.List<WarehouseModel>> Execute()
        {
            var rows = await _warehouses.EnsureAll();
            return rows.Select(row => new WarehouseModel
            {
                Id = row.Id,
                Name = row.Name,
                Kind = row.Kind,
                KindName = row.Kind.GetEnumDescription()
            }).ToList();
        }
    }
}
