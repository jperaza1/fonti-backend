using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Category.Commands.EnableCategory
{
    public class EnableCategoryCommand : BaseCommand, IEnableCategoryCommand
    {
        public EnableCategoryCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
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
                    IsActive = true
                });

            await _repository.CommitAsync();
        }
    }
}
