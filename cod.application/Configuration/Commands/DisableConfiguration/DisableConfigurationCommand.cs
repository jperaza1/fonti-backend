using System.Threading.Tasks;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;

namespace cod.application.Configuration.Commands.DisableConfiguration
{
    public class DisableConfigurationCommand : BaseCommand, IDisableConfigurationCommand
    {
        public DisableConfigurationCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(int id)
        {
            bool exist = await _repository.AnyAsync<domain.General.Configuration>(x => x.Id == id);

            if (!exist)
                throw new ApplicationException("CONFIGURATION NOT FOUND", "CONFIGURATION");

            await _repository.UpdateAsync<domain.General.Configuration>(
                conf => conf.Id == id,
                conf => new domain.General.Configuration()
                {
                    IsActive = false
                }
                );

            await _repository.CommitAsync();
        }
    }
}
