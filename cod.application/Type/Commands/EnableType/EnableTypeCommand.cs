using System.Threading.Tasks;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;

namespace cod.application.Type.Commands.EnableType
{
    public class EnableTypeCommand : BaseCommand, IEnableTypeCommand
    {
        public EnableTypeCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(int id)
        {
            var exist = await _repository.AnyAsync<domain.Data.Type>(x => x.Id == id);

            if (!exist)
                throw new ApplicationException("TYPE NOT FOUND", "TYPE");

            await _repository.UpdateAsync<domain.Data.Type>(
                type => type.Id == id,
                type => new domain.Data.Type
                {
                    IsActive = true
                }
                );

            await _repository.CommitAsync();
        }
    }
}
