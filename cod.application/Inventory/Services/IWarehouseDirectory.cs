using cod.domain.Features.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.Inventory.Services
{
    public interface IWarehouseDirectory
    {
        Task<List<domain.Operation.Warehouse>> EnsureAll();
        Task<domain.Operation.Warehouse> Require(WarehouseKind kind);
        Task<domain.Operation.Warehouse> Get(int id);
        bool Allows(WarehouseKind kind, ProductType type);
    }
}
