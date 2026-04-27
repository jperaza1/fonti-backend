using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Dish.Commands.RemoveDishCafeteria
{
    public class RemoveDishCafeteriaCommand : BaseCommand, IRemoveDishCafeteriaCommand
    {
        public RemoveDishCafeteriaCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(int id)
        {
            var dish = await _repository.Query<domain.SalePoint.DishCafeteria>().FirstOrDefaultAsync(f => f.Id == id);

            if (dish == null)
                throw new ApplicationException("Registro no encontrado", "PLATO");

            _repository.DeleteSync(dish);

            await _repository.CommitAsync();
        }
    }
}
