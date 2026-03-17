using cod.application.Alerts.Commands.CreateAlertConfiguration;
using cod.application.EnumDefinition.Queries.GetCarrierType;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.Alerts.Queries.GetAlertConfigurationById
{
    public class GetAlertConfigurationByIdQuery : BaseQuery, IGetAlertConfigurationByIdQuery
    {
        public GetAlertConfigurationByIdQuery(IRepository repository) : base(repository)
        { }

        public async Task<GetAlertConfigurationByIdModel> Execute(int id)
        {
            var alert = await this._repository.FirstOrDefaultAsync<domain.Alerts.AlertConfiguration>(x => x.Id == id);

            return new GetAlertConfigurationByIdModel
            {
                Id = alert.Id,
                AlertBy = alert.AlertBy,
                Name = alert.Name,
                AlertTypes = !string.IsNullOrEmpty(alert.AlertTypes) ? alert.AlertTypes.Split(',').Select(value => (UserAlertOption)Enum.Parse(typeof(UserAlertOption), value)).ToList() : new List<UserAlertOption>(),
                CustomerTypes = !string.IsNullOrEmpty(alert.CustomerTypes) ? alert.CustomerTypes.Split(',').Select(value => (domain.Features.Enums.LeadCustomerType)Enum.Parse(typeof(domain.Features.Enums.LeadCustomerType), value)).ToList() : new List<domain.Features.Enums.LeadCustomerType>(),
                FrequencyDaysAfter = !string.IsNullOrEmpty(alert.FrequencyDaysAfter) ? alert.FrequencyDaysAfter.Split(',').Select(value => int.Parse(value)).ToList() : new List<int>(),
                FrequencyDaysBefore = !string.IsNullOrEmpty(alert.FrequencyDaysBefore) ? alert.FrequencyDaysBefore.Split(',').Select(value => int.Parse(value)).ToList() : new List<int>(),
                BodyType = alert.BodyType,
                NotifyAfter = alert.NotifyAfter,
                NotifyBefore = alert.NotifyBefore,
                HolidayDate = alert.HolidayDate,
                Languages = alert.AlertConfigurationLanguages.Select(x => new AlertConfigurationLanguageModel
                {
                    Id = x.Id,
                    AlertConfigurationId = x.AlertConfigurationId,
                    BodyFields = x.BodyFields,
                    BodyTemplate = x.BodyTemplate,
                    LanguageType = x.LanguageType
                })
                .ToList()
            };
        }
    }
}
