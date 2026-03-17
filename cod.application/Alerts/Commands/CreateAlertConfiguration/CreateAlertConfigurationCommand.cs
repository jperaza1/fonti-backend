using cod.application.Alerts.Commands.UpdateAlertConfiguration;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.domain.Alerts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.Alerts.Commands.CreateAlertConfiguration
{
    public class CreateAlertConfigurationCommand : BaseCommand, ICreateAlertConfigurationCommand
    {

        public CreateAlertConfigurationCommand(
            IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        { }

        public async Task Execute(CreateAlertConfigurationModel model)
        {
            ValidateModel(model);

            var added = new AlertConfiguration
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
                AlertConfigurationLanguages = this.GetDetail(model),
                HolidayDate = model.HolidayDate
            };

            _repository.InsertAsync(added);
            await _repository.CommitAsync();
        }

        public List<AlertConfigurationLanguage> GetDetail(CreateAlertConfigurationModel model)
        {
            var listAdded = new List<AlertConfigurationLanguage>();
            foreach (var item in model.Languages)
            {
                var added = new AlertConfigurationLanguage
                {
                    BodyFields = item.BodyFields,
                    BodyTemplate = item.BodyTemplate,
                    LanguageType = item.LanguageType
                };
                _repository.SetAuthValuesToCreate(added);
                listAdded.Add(added);
            }
            return listAdded;
        }

        private void ValidateModel(CreateAlertConfigurationModel model)
        {
            if (model.AlertBy == "Holiday" && !model.HolidayDate.HasValue)
                this._exceptionFactory.Add("holidayDate",
                    this._exceptionFactory.GetMessage("Holiday Date", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new Features.Exception.ApplicationException("Transaction canceled", "ALERT EMAIL", this._exceptionFactory.Exceptions);
        }
    }
}
