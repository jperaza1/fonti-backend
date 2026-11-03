using cod.application.Production.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.Inventory.Services
{
    public interface IInventoryMovementService
    {
        Task ApplyDispatch(cod.domain.Operation.DispatchControl dispatch);
        Task ApplyDetail(cod.domain.Operation.DispatchControl dispatch, cod.domain.Operation.DispatchControlDetail detail);
        Task ApplyEntry(int productId, decimal quantity, System.DateTime movementDate, string note, decimal? unitCost);
        Task ApplyExit(int productId, decimal quantity, System.DateTime movementDate, string note, decimal? unitCost);
        Task ApplyQualityExit(int productId, decimal quantity, System.DateTime movementDate, string note, decimal? unitCost);
        Task<List<ProductionConsumptionModel>> GetConsumption(int productId, decimal quantity);
        Task ApplyProduction(cod.domain.Operation.ProductionOrder order, string shiftName, List<ProductionLossModel> losses);
        Task ReverseProduction(cod.domain.Operation.ProductionOrder order);
    }
}
