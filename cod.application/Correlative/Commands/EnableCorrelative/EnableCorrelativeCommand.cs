using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Correlative.Commands.EnableCorrelative
{
    public class EnableCorrelativeCommand : BaseCommand, IEnableCorrelativeCommand
    {
        public EnableCorrelativeCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
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
                    IsActive = true
                });

            await _repository.CommitAsync();
        }
    }
}
