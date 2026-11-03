using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.PurchaseOrder.Commands.RejectPurchaseOrder;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.PurchaseOrder.Commands.CancelPurchaseOrder
{
    public class CancelPurchaseOrderCommand : BaseCommand, ICancelPurchaseOrderCommand
    {
        private readonly AuthService _authService;

        public CancelPurchaseOrderCommand(IRepository repository, IExceptionFactory exceptionFactory, AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task Execute(RejectPurchaseOrderModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Reason))
                throw new ApplicationException("Es requerido ingresar una razón", "ORDEN COMPRA");

            var purchaseOrder = await _repository.WhereAsync<domain.Operation.PurchaseOrder>(item => item.Id == model.PurchaseOrderId && item.IsActive)
                .FirstOrDefaultAsync();

            if (purchaseOrder == null)
                throw new ApplicationException("Orden de compra no existe", "ORDEN COMPRA");

            purchaseOrder.OrderStatus = PurchaseOrderStatus.Canceled;
            purchaseOrder.ReasonCanceled = model.Reason;
            purchaseOrder.CanceledAt = Clock.Now;
            purchaseOrder.CanceledUser = _authService.User;
            _repository.UpdateSync(purchaseOrder);
            await _repository.CommitAsync();
        }
    }
}
