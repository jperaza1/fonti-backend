using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.PurchaseOrder.Commands.RejectPurchaseOrder;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.OfficeRequisition.Commands.RejectOfficeRequisition
{
    public class RejectOfficeRequisitionCommand : BaseCommand, IRejectOfficeRequisitionCommand
    {
        private readonly AuthService _authService;
        public RejectOfficeRequisitionCommand(
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

            var officeRequisition = await _repository.AllActive<domain.Operation.OfficeRequisition>()
                .Where(f => f.Id == model.OfficeRequisitionId).FirstOrDefaultAsync();

            if (officeRequisition == null)
                throw new ApplicationException("Requisicion de oficina no existe", "CANCELED");

            officeRequisition.Status = domain.Features.Enums.DomainEnums.SpecialServiceStatus.Rejected;
            officeRequisition.ReasonRejection = model.Reason;
            officeRequisition.RejectedAt = Clock.Now;
            officeRequisition.RejectedUser = _authService.User;

            _repository.UpdateSync(officeRequisition);

            await _repository.CommitAsync();
        }
    }
}
