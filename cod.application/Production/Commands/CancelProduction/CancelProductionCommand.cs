using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Inventory.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Production.Commands.CancelProduction
{
    public interface ICancelProductionCommand
    {
        Task Execute(int id);
    }

    public class CancelProductionCommand : BaseCommand, ICancelProductionCommand
    {
        private readonly IInventoryMovementService _inventoryMovementService;

        public CancelProductionCommand(
            IRepository repository,
            IExceptionFactory exceptionFactory,
            IInventoryMovementService inventoryMovementService) : base(repository, exceptionFactory)
        {
            _inventoryMovementService = inventoryMovementService;
        }

        public async Task Execute(int id)
        {
            var order = await _repository.WhereAsync<domain.Operation.ProductionOrder>(o => o.Id == id)
                .FirstOrDefaultAsync();

            if (order == null || !order.IsActive)
            {
                _exceptionFactory.Add("id", "La producción no existe o ya está anulada");
                throw new ApplicationException("CANCELED", "PRODUCCION", _exceptionFactory.Exceptions);
            }

            if (order.ProductionShiftId != null)
            {
                var parent = await _repository.WhereAsync<domain.Operation.ProductionShift>(s => s.Id == order.ProductionShiftId)
                    .Select(s => new { s.IsActive, s.IsClosed })
                    .FirstOrDefaultAsync();

                if (parent != null && (!parent.IsActive || parent.IsClosed))
                {
                    _exceptionFactory.Add("id", parent.IsClosed ? "El turno ya está cerrado" : "El turno ya fue anulado");
                    throw new ApplicationException("CANCELED", "PRODUCCION", _exceptionFactory.Exceptions);
                }
            }

            await _inventoryMovementService.ReverseProduction(order);
            _repository.DisableSync(order);
            await _repository.CommitAsync();
        }
    }
}
