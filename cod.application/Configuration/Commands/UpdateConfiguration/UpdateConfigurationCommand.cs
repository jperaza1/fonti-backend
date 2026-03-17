using System.Threading.Tasks;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;

namespace cod.application.Configuration.Commands.UpdateConfiguration
{
    public class UpdateConfigurationCommand : BaseCommand, IUpdateConfigurationCommand
    {
        public UpdateConfigurationCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(UpdateConfigurationModel model, int id)
        {
            Validate(model);

            bool exist = await _repository.AnyAsync<domain.General.Configuration>(x => x.Id == id);

            if (!exist)
                throw new ApplicationException("CONFIGURATION NOT FOUND", "CONFIGURATION");

            await _repository.UpdateAsync<domain.General.Configuration>(
                conf => conf.Id == id,
                conf => new domain.General.Configuration()
                {
                    Value = model.Value,
                }
                );

            await _repository.CommitAsync();
        }

        private void Validate(UpdateConfigurationModel model)
        {
            if (string.IsNullOrEmpty(model.Value))
                this._exceptionFactory.Add("value",
                    this._exceptionFactory.GetMessage("value", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("Transaction canceled", "CONFIGURATION", this._exceptionFactory.Exceptions);

        }
    }
}
