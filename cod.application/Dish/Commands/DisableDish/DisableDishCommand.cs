using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Dish.Commands.DisableDish
{
    public class DisableDishCommand : BaseCommand, IDisableDishCommand
    {
        public DisableDishCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(int id)
        {
            var exist = await _repository.AnyAsync<domain.SalePoint.Dish>(x => x.Id == id);

            if (!exist)
                throw new ApplicationException("PLATO", "PLATO");

            await _repository.UpdateAsync<domain.SalePoint.Dish>(
                type => type.Id == id,
                type => new domain.SalePoint.Dish
                {
                    IsActive = false
                });

            await _repository.CommitAsync();
        }
    }
}
