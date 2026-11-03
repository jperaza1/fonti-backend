using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Distributor.Commands.DisableDistributor
{
    public class DisableDistributorCommand : BaseCommand, IDisableDistributorCommand
    {
        public DisableDistributorCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(int id)
        {
            var exist = await _repository.AnyAsync<domain.Data.Distributor>(x => x.Id == id);

            if (!exist)
                throw new ApplicationException("DISTRIBUIDOR NO EXISTE", "DISTRIBUIDOR");

            await _repository.UpdateAsync<domain.Data.Distributor>(
                type => type.Id == id,
                type => new domain.Data.Distributor
                {
                    IsActive = false
                });

            await _repository.CommitAsync();
        }
    }
}
