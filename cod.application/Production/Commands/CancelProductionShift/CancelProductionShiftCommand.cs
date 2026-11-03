using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Inventory.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Production.Commands.CancelProductionShift
{
    public interface ICancelProductionShiftCommand
    {
        Task Execute(int id);
    }

    public class CancelProductionShiftCommand : BaseCommand, ICancelProductionShiftCommand
    {
        private readonly IInventoryMovementService _inventoryMovementService;

        public CancelProductionShiftCommand(
            IRepository repository,
            IExceptionFactory exceptionFactory,
            IInventoryMovementService inventoryMovementService) : base(repository, exceptionFactory)
        {
            _inventoryMovementService = inventoryMovementService;
        }

        public async Task Execute(int id)
        {
            var shift = await _repository.WhereAsync<domain.Operation.ProductionShift>(s => s.Id == id)
                .FirstOrDefaultAsync();

            if (shift == null || !shift.IsActive)
            {
                _exceptionFactory.Add("id", "El turno no existe o ya está anulado");
                throw new ApplicationException("CANCELED", "PRODUCCION", _exceptionFactory.Exceptions);
            }

            if (shift.IsClosed)
            {
                _exceptionFactory.Add("id", "El turno ya está cerrado");
                throw new ApplicationException("CANCELED", "PRODUCCION", _exceptionFactory.Exceptions);
            }

            var lines = await _repository.WhereAsync<domain.Operation.ProductionOrder>(o => o.ProductionShiftId == id && o.IsActive)
                .ToListAsync();

            foreach (var line in lines)
            {
                await _inventoryMovementService.ReverseProduction(line);
                _repository.DisableSync(line);
            }

            _repository.DisableSync(shift);
            await _repository.CommitAsync();
        }
    }
}
