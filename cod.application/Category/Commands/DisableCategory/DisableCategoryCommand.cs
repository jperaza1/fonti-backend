using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Category.Commands.DisableCategory
{
    public class DisableCategoryCommand : BaseCommand, IDisableCategoryCommand
    {
        public DisableCategoryCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(int id)
        {
            var exist = await _repository.AnyAsync<domain.Data.Category>(x => x.Id == id);

            if (!exist)
                throw new ApplicationException("CATEGORÍA NO EXISTE", "CATEGORIA");

            await _repository.UpdateAsync<domain.Data.Category>(
                type => type.Id == id,
                type => new domain.Data.Category
                {
                    IsActive = false
                });

            await _repository.CommitAsync();
        }
    }
}
