using System.Threading.Tasks;
using cod.application.Features.Exception;
using cod.application.Interfaces;

namespace cod.application.TypeGroup.Commands.DisableTypeGroup
{
    public class DisableTypeGroupCommand : IDisableTypeGroupCommand
    {
        private readonly IRepository _repository;

        public DisableTypeGroupCommand(IRepository repository)
        {
            _repository = repository;
        }

        public async Task Execute(int id)
        {
            await ValidateRegistry(id);

            await _repository.UpdateAsync<domain.Data.TypeGroup>(model =>
                model.Id == id,
                newValues => new domain.Data.TypeGroup
                {
                    IsActive = false
                });

            await _repository.CommitAsync();
        }
        private async Task ValidateRegistry(int id)
        {
            bool existRegistry = await _repository
                .AnyAsync<domain.Data.TypeGroup>(model => model.Id == id);

            if (!existRegistry)
                throw new ApplicationException("TYPEGROUP NOT FOUND", "TYPEGROUP");
        }

    }
}
