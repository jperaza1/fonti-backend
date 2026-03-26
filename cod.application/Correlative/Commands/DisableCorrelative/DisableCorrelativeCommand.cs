using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Correlative.Commands.DisableCorrelative
{
    public class DisableCorrelativeCommand : BaseCommand, IDisableCorrelativeCommand
    {
        public DisableCorrelativeCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(int id)
        {
            var exist = await _repository.AnyAsync<domain.Data.Correlative>(x => x.Id == id);

            if (!exist)
                throw new ApplicationException("CORRELATIVO NO EXISTE", "CORRELATIVO");

            await _repository.UpdateAsync<domain.Data.Correlative>(
                type => type.Id == id,
                type => new domain.Data.Correlative
                {
                    IsActive = false
                });

            await _repository.CommitAsync();
        }
    }
}
