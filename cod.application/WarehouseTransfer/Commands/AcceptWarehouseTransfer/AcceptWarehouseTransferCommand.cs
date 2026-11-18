using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Inventory.Services;
using cod.domain.Features;
using cod.domain.Features.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.WarehouseTransfer.Commands.AcceptWarehouseTransfer
{
    public class AcceptWarehouseTransferCommand : BaseCommand, IAcceptWarehouseTransferCommand
    {
        private readonly AuthService _authService;
        private readonly IInventoryMovementService _inventory;

        public AcceptWarehouseTransferCommand(
            IRepository repository,
            IExceptionFactory exceptionFactory,
            AuthService authService,
            IInventoryMovementService inventory) : base(repository, exceptionFactory)
        {
            _authService = authService;
            _inventory = inventory;
        }

        public async Task Execute(int id)
        {
            var transfer = await Load(id);
            var origin = transfer.FromWarehouse.Name;

            foreach (var line in transfer.Details.Where(detail => detail.IsActive && detail.Quantity > 0))
            {
                var name = line.Product == null ? "Producto " + line.ProductId : line.Product.Name;
                await _inventory.ApplyTransferIn(
                    transfer.ToWarehouseId,
                    line.ProductId,
                    line.Quantity,
                    Clock.Now,
                    "Transferencia de " + origin + " (" + name + ")",
                    line.Product == null ? null : line.Product.Cost);
            }

            transfer.Status = WarehouseTransferStatus.Accepted;
            transfer.AcceptedUser = _authService.User;
            transfer.AcceptedAt = Clock.Now;
            _repository.UpdateSync(transfer);
            await _repository.CommitAsync();
        }

        private async Task<domain.Operation.WarehouseTransfer> Load(int id)
        {
            var transfer = await _repository.QueryActive<domain.Operation.WarehouseTransfer>()
                .Include(item => item.FromWarehouse)
                .Include(item => item.Details)
                .ThenInclude(detail => detail.Product)
                .FirstOrDefaultAsync(item => item.Id == id);

            if (transfer == null)
                throw new ApplicationException("La transferencia no existe", "TRANSFERENCIA");

            if (transfer.Status != WarehouseTransferStatus.Pending)
                throw new ApplicationException("La transferencia ya no está pendiente", "TRANSFERENCIA");

            return transfer;
        }
    }
}
