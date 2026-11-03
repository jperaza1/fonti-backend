using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Inventory.Services;
using cod.application.Production.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Production.Queries.GetProductionConsumption
{
    public interface IGetProductionConsumptionQuery
    {
        Task<List<ProductionConsumptionModel>> Execute(ProductionConsumptionRequest request);
    }

    public class GetProductionConsumptionQuery : BaseQuery, IGetProductionConsumptionQuery
    {
        private readonly IInventoryMovementService _inventoryMovementService;
        private readonly IExceptionFactory _exceptionFactory;

        public GetProductionConsumptionQuery(
            IRepository repository,
            IExceptionFactory exceptionFactory,
            IInventoryMovementService inventoryMovementService) : base(repository)
        {
            _exceptionFactory = exceptionFactory;
            _inventoryMovementService = inventoryMovementService;
        }

        public Task<List<ProductionConsumptionModel>> Execute(ProductionConsumptionRequest request)
        {
            if (request == null || request.ProductId <= 0)
                _exceptionFactory.Add("productId", "El producto es requerido");

            if (request == null || request.Quantity <= 0)
                _exceptionFactory.Add("quantity", "La cantidad debe ser mayor a cero");

            if (!_exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "PRODUCCION", _exceptionFactory.Exceptions);

            return _inventoryMovementService.GetConsumption(request.ProductId, request.Quantity);
        }
    }
}
