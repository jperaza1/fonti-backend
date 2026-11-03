using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Production.Commands.CloseProductionShift
{
    public interface ICloseProductionShiftCommand
    {
        Task Execute(int id);
    }

    public class CloseProductionShiftCommand : BaseCommand, ICloseProductionShiftCommand
    {
        public CloseProductionShiftCommand(IRepository repository, IExceptionFactory exceptionFactory)
            : base(repository, exceptionFactory)
        {
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

            shift.IsClosed = true;
            _repository.UpdateSync(shift);
            await _repository.CommitAsync();
        }
    }
}
