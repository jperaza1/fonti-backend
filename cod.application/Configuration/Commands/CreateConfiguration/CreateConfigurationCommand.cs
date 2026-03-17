using System.Threading.Tasks;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;

namespace cod.application.Configuration.Commands.CreateConfiguration
{
    public class CreateConfigurationCommand : BaseCommand, ICreateConfigurationCommand
    {
        public CreateConfigurationCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(CreateConfigurationModel model)
        {
            await Validate(model);

            domain.General.Configuration configuration = new domain.General.Configuration
            {
                Key = model.Key,
                Value = model.Value
            };

            _repository.InsertAsync(configuration);

            await _repository.CommitAsync();
        }

        private async Task Validate(CreateConfigurationModel model)
        {
            if (string.IsNullOrEmpty(model.Key))
                this._exceptionFactory.Add("key",
                    this._exceptionFactory.GetMessage("key", ExceptionEnums.ExceptionCode.Required));

            if (string.IsNullOrEmpty(model.Value))
                this._exceptionFactory.Add("value",
                    this._exceptionFactory.GetMessage("value", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("Transaction canceled", "CONFIGURATION", this._exceptionFactory.Exceptions);

            bool existTermWithSameKey = await _repository.AnyAsync<domain.General.Configuration>(term => term.Key.ToUpper() == model.Key.ToUpper());

            if (existTermWithSameKey)
                this._exceptionFactory.Add("key",
                    this._exceptionFactory.GetMessage("key", ExceptionEnums.ExceptionCode.Duplicate));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("Transaction canceled", "CONFIGURATION", this._exceptionFactory.Exceptions);

        }
    }
}
