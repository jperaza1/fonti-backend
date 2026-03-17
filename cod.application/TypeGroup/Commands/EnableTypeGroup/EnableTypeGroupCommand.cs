using System.Threading.Tasks;
using cod.application.Features.Exception;
using cod.application.Interfaces;

namespace cod.application.TypeGroup.Commands.EnableTypeGroup
{
    public class EnableTypeGroupCommand : IEnableTypeGroupCommand
    {
        private readonly IRepository _repository;

        public EnableTypeGroupCommand(IRepository repository)
        {
            _repository = repository;
        }

        public async Task Execute(int id)
        {
            await ValidateRegistry(id);

            await _repository.UpdateAsync<domain.Data.TypeGroup>(model => model.Id == id,
                newValues => new domain.Data.TypeGroup
                {
                    IsActive = true
                });

            await _repository.CommitAsync();

        }

        private async Task ValidateRegistry(int id)
        {
            bool existRegistry = await _repository.AnyAsync<domain.Data.TypeGroup>(model => model.Id == id);

            if (!existRegistry)
                throw new ApplicationException("TYPE GROUP NOT FOUND", "TYPEGROUP");
        }
    }
}
