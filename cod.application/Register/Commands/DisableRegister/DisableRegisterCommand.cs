using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Register.Commands.DisableRegister
{
    public class DisableRegisterCommand : BaseCommand, IDisableRegisterCommand
    {
        public DisableRegisterCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(int id)
        {
            var exist = await _repository.AnyAsync<domain.Data.Register>(x => x.Id == id);

            if (!exist)
                throw new ApplicationException("CAJA NO EXISTE", "CAJA");

            await _repository.UpdateAsync<domain.Data.Register>(
                type => type.Id == id,
                type => new domain.Data.Register
                {
                    IsActive = false
                }
                );

            await _repository.CommitAsync();
        }
    }
}
