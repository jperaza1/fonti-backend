using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Inventory.Services;
using cod.application.WarehouseTransfer.Common;
using cod.domain.Features;
using cod.domain.Features.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.WarehouseTransfer.Commands.RejectWarehouseTransfer
{
    public class RejectWarehouseTransferCommand : BaseCommand, IRejectWarehouseTransferCommand
    {
        private readonly AuthService _authService;
        private readonly IInventoryMovementService _inventory;

        public RejectWarehouseTransferCommand(
            IRepository repository,
            IExceptionFactory exceptionFactory,
            AuthService authService,
            IInventoryMovementService inventory) : base(repository, exceptionFactory)
        {
            _authService = authService;
            _inventory = inventory;
        }

        public async Task Execute(RejectWarehouseTransferModel model)
        {
            var transfer = await _repository.QueryActive<domain.Operation.WarehouseTransfer>()
                .Include(item => item.ToWarehouse)
                .Include(item => item.Details)
                .ThenInclude(detail => detail.Product)
                .FirstOrDefaultAsync(item => item.Id == (model == null ? 0 : model.Id));

            if (transfer == null)
                throw new ApplicationException("La transferencia no existe", "TRANSFERENCIA");

            if (transfer.Status != WarehouseTransferStatus.Pending)
                throw new ApplicationException("La transferencia ya no está pendiente", "TRANSFERENCIA");

            var destination = transfer.ToWarehouse == null ? "la bodega destino" : transfer.ToWarehouse.Name;

            foreach (var line in transfer.Details.Where(detail => detail.IsActive && detail.Quantity > 0))
            {
                var name = line.Product == null ? "Producto " + line.ProductId : line.Product.Name;
                await _inventory.ApplyTransferIn(
                    transfer.FromWarehouseId,
                    line.ProductId,
                    line.Quantity,
                    Clock.Now,
                    "Rechazo de transferencia a " + destination + " (" + name + ")",
                    line.Product == null ? null : line.Product.Cost);
            }

            transfer.Status = WarehouseTransferStatus.Rejected;
            transfer.RejectedUser = _authService.User;
            transfer.RejectedAt = Clock.Now;
            transfer.Reason = model == null || string.IsNullOrWhiteSpace(model.Reason) ? null : model.Reason.Trim();
            _repository.UpdateSync(transfer);
            await _repository.CommitAsync();
        }
    }
}
