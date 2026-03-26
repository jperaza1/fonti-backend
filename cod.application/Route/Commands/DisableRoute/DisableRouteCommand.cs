using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Route.Commands.DisableRoute
{
    public class DisableRouteCommand : BaseCommand, IDisableRouteCommand
    {
        public DisableRouteCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(int id)
        {
            var exist = await _repository.AnyAsync<domain.Data.Route>(x => x.Id == id);

            if (!exist)
                throw new ApplicationException("RUTA NO EXISTE", "RUTA");

            await _repository.UpdateAsync<domain.Data.Route>(
                type => type.Id == id,
                type => new domain.Data.Route
                {
                    IsActive = false
                });

            await _repository.CommitAsync();
        }
    }
}
