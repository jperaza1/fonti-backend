using cod.application.DispatchControl.Services;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.domain.Operation;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.DispatchControl.Commands.CancelDispatchTransfer
{
    public class CancelDispatchTransferCommand : BaseCommand, ICancelDispatchTransferCommand
    {
        private readonly IDispatchTransferSettings _transferSettings;

        public CancelDispatchTransferCommand(
            IRepository repository,
            IExceptionFactory exceptionFactory,
            IDispatchTransferSettings transferSettings) : base(repository, exceptionFactory)
        {
            _transferSettings = transferSettings;
        }

        public async Task Execute(int id)
        {
            if (!await _transferSettings.IsEnabled())
            {
                this._exceptionFactory.Add("transfer", "El traspaso entre rutas está apagado.");
                throw new ApplicationException("CANCELED", "Control de Salida", this._exceptionFactory.Exceptions);
            }

            var transfer = await _repository.WhereActive<DispatchTransfer>(item => item.Id == id)
                .FirstOrDefaultAsync();

            if (transfer == null)
            {
                this._exceptionFactory.Add("transfer", "El traspaso no existe.");
                throw new ApplicationException("CANCELED", "Control de Salida", this._exceptionFactory.Exceptions);
            }

            var details = await _repository.WhereActive<DispatchControlDetail>(detail => detail.DispatchTransferId == id)
                .Include(detail => detail.ProductMovements)
                .ToListAsync();

            var controlIds = details.Select(detail => detail.DispatchControlId).Distinct().ToList();
            controlIds.Add(transfer.FromDispatchControlId);
            controlIds.Add(transfer.ToDispatchControlId);

            var closed = await _repository.Query<domain.Operation.DispatchControl>()
                .AnyAsync(control => controlIds.Contains(control.Id)
                    && control.Status != DispatchControlStatus.Open);

            if (closed)
            {
                this._exceptionFactory.Add("transfer", "Solo se anula el traspaso si las dos rutas siguen con el control abierto.");
                throw new ApplicationException("CANCELED", "Control de Salida", this._exceptionFactory.Exceptions);
            }

            foreach (var detail in details)
            {
                if (detail.ProductMovements != null)
                {
                    foreach (var movement in detail.ProductMovements.Where(movement => movement.IsActive))
                        _repository.DisableSync(movement);
                }

                _repository.DisableSync(detail);
            }

            _repository.DisableSync(transfer);
            await _repository.CommitAsync();
        }
    }
}
