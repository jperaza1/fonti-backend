using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.PurchaseOrder.Commands.RejectPurchaseOrder
{
    public class RejectPurchaseOrderCommand : BaseCommand, IRejectPurchaseOrderCommand
    {
        private readonly AuthService _authService;
        public RejectPurchaseOrderCommand(
            IRepository repository, 
            IExceptionFactory exceptionFactory,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task Execute(RejectPurchaseOrderModel model)
        {
            if (string.IsNullOrEmpty(model.Reason))
                throw new ApplicationException("Es requerido ingresar una razon para rechazar", "CANCELED");

            var purchaseOrder = await _repository.AllActive<domain.Operation.PurchaseOrder>()
                .Where(f => f.Id == model.PurchaseOrderId).FirstOrDefaultAsync();

            if (purchaseOrder == null)
                throw new ApplicationException("Odern de Compa no existe", "CANCELED");

            purchaseOrder.OrderStatus = domain.Features.Enums.DomainEnums.PurchaseOrderStatus.Rejected;
            purchaseOrder.ReasonRejection = model.Reason;
            purchaseOrder.RejectedAt = Clock.Now;
            purchaseOrder.RejectedUser = _authService.User;

            _repository.UpdateSync(purchaseOrder);

            await _repository.CommitAsync();
        }
    }
}
