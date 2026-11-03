using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Truck.Commands.DisableTruck
{
    public class DisableTruckCommand : BaseCommand, IDisableTruckCommand
    {
        public DisableTruckCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(int id)
        {
            var exist = await _repository.AnyAsync<domain.Data.Truck>(x => x.Id == id);

            if (!exist)
                throw new ApplicationException("CAMION NO EXISTE", "CAMION");

            await _repository.UpdateAsync<domain.Data.Truck>(
                type => type.Id == id,
                type => new domain.Data.Truck
                {
                    IsActive = false
                });

            await _repository.CommitAsync();
        }
    }
}
