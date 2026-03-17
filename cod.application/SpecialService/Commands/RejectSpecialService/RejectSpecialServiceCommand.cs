using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.PurchaseOrder.Commands.RejectPurchaseOrder;
using cod.domain.Features;
using cod.domain.Operation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.SpecialService.Commands.RejectSpecialService
{
    public class RejectSpecialServiceCommand : BaseCommand, IRejectSpecialServiceCommand
    {
        private readonly AuthService _authService;
        public RejectSpecialServiceCommand(
            IRepository repository, 
            IExceptionFactory exceptionFactory,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task Execute(RejectPurchaseOrderModel model)
        {
            if(string.IsNullOrEmpty(model.Reason))
                throw new ApplicationException("Es requerido ingresar una razon para rechazar", "CANCELED");

            var specialService = await _repository.AllActive<domain.Operation.SpecialService>()
                .Where(f => f.Id == model.SpecialServiceId).FirstOrDefaultAsync();

            if (specialService == null)
                throw new ApplicationException("Servicio especial no existe", "CANCELED");

            specialService.Status = domain.Features.Enums.DomainEnums.SpecialServiceStatus.Rejected;
            specialService.ReasonRejection = model.Reason;
            specialService.RejectedAt = Clock.Now;
            specialService.RejectedUser = _authService.User;

            _repository.UpdateSync(specialService);

            await _repository.CommitAsync();
        }
    }
}
