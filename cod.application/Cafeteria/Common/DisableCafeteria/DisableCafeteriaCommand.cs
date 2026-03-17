using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Cafeteria.Common.DisableCafeteria
{
    public class DisableCafeteriaCommand : BaseCommand, IDisableCafeteriaCommand
    {
        public DisableCafeteriaCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(int id)
        {
            var exist = await _repository.AnyAsync<domain.Data.Cafeteria>(x => x.Id == id);

            if (!exist)
                throw new ApplicationException("CAFETERIA NO EXISTE", "CAFETERIA");

            await _repository.UpdateAsync<domain.Data.Cafeteria>(
                type => type.Id == id,
                type => new domain.Data.Cafeteria
                {
                    IsActive = false
                }
                );

            await _repository.CommitAsync();
        }
    }
}
