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

namespace cod.application.SpecialService.Commands.UpdateStatus
{
    public class UpdateStatusSpecialServiceCommand : BaseCommand, IUpdateStatusSpecialServiceCommand
    {
        private readonly AuthService _authService;
        public UpdateStatusSpecialServiceCommand(IRepository repository, IExceptionFactory exceptionFactory,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task Execute(int id)
        {
            var specialService = await _repository.QueryActive<domain.Operation.SpecialService>().Where(f => f.Id == id).FirstOrDefaultAsync();

            if (specialService == null) throw new ApplicationException("Servicio Especial no existe", "SERVICIO ESPECIAL");

            switch (specialService.Status)
            {
                case domain.Features.Enums.DomainEnums.SpecialServiceStatus.Created:
                    specialService.Status = domain.Features.Enums.DomainEnums.SpecialServiceStatus.Approved;
                    specialService.ApprovedAt = DateTime.Now;
                    specialService.ApprovedUser = _authService.User;
                    break;
            }
            _repository.UpdateSync(specialService);
            await _repository.CommitAsync();
        }
    }
}
