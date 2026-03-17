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

namespace cod.application.OfficeRequisition.Commands.UpdateStatus
{
    public class UpdateStatusOfficeRequisitionCommand : BaseCommand, IUpdateStatusOfficeRequisitionCommand
    {
        private readonly AuthService _authService;
        public UpdateStatusOfficeRequisitionCommand(
            IRepository repository, 
            IExceptionFactory exceptionFactory,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task Execute(int id)
        {
            var officeRequisition = await _repository.QueryActive<domain.Operation.OfficeRequisition>().Where(f => f.Id == id).FirstOrDefaultAsync();

            if (officeRequisition == null) throw new ApplicationException("Requisicion de oficina no existe", "SERVICIO ESPECIAL");

            switch (officeRequisition.Status)
            {
                case domain.Features.Enums.DomainEnums.SpecialServiceStatus.Created:
                    officeRequisition.Status = domain.Features.Enums.DomainEnums.SpecialServiceStatus.Approved;
                    officeRequisition.ApprovedAt = DateTime.Now;
                    officeRequisition.ApprovedUser = _authService.User;
                    break;
            }
            _repository.UpdateSync(officeRequisition);
            await _repository.CommitAsync();
        }
    }
}
