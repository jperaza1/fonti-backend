using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.Alerts.Commands.UpdateAlertConfiguration
{
    public class UpdateAlertConfigurationCommand : BaseCommand, IUpdateAlertConfigurationCommand
    {
        public UpdateAlertConfigurationCommand(
            IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        { }

        public async Task Execute(UpdateAlertConfigurationModel model, int id)
        {
            ValidateModel(model);


            await _repository
                .UpdateAsync<domain.Alerts.AlertConfiguration>(exp => exp.Id == id,
                reg => new domain.Alerts.AlertConfiguration
                {
                    Name = model.Name,
                    BodyType = model.BodyType,
                    AlertBy = model.AlertBy,
                    AlertTypes = model.AlertTypes.Any() ? model.AlertTypes.Select(integer => integer.ToString()).Aggregate((a, b) => $"{a},{b}") : string.Empty,
                    FrequencyDaysAfter = model.FrequencyDaysAfter.Any() ? model.FrequencyDaysAfter.Select(integer => integer.ToString()).Aggregate((a, b) => $"{a},{b}") : string.Empty,
                    FrequencyDaysBefore = model.FrequencyDaysBefore.Any() ? model.FrequencyDaysBefore.Select(integer => integer.ToString()).Aggregate((a, b) => $"{a},{b}") : string.Empty,
                    CustomerTypes = model.CustomerTypes.Any() ? model.CustomerTypes.Select(integer => integer.ToString()).Aggregate((a, b) => $"{a},{b}") : string.Empty,
                    NotifyAfter = model.NotifyAfter,
                    NotifyBefore = model.NotifyBefore,
                    HolidayDate = model.HolidayDate
                });

            foreach (var item in model.Languages)
            {
                await _repository
                        .UpdateAsync<domain.Alerts.AlertConfigurationLanguage>(
                                                      exp => exp.Id == item.Id,
                                                      reg => new domain.Alerts.AlertConfigurationLanguage
                                                      {
                                                          BodyFields = item.BodyFields,
                                                          BodyTemplate = item.BodyTemplate,
                                                          LanguageType = item.LanguageType
                                                      });
            }


            await _repository.CommitAsync();
        }

        private void ValidateModel(UpdateAlertConfigurationModel model)
        {
            if(model.AlertBy == "Holiday" && !model.HolidayDate.HasValue)
                this._exceptionFactory.Add("holidayDate",
                    this._exceptionFactory.GetMessage("Holiday Date", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("Transaction canceled", "ALERT EMAIL", this._exceptionFactory.Exceptions);
        }
    }
}
