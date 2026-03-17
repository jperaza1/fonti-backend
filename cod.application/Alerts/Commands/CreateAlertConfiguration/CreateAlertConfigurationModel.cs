using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.Alerts.Commands.CreateAlertConfiguration
{
    public class CreateAlertConfigurationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AlertBy { get; set; }
        public string BodyType { get; set; }
        public List<UserAlertOption> AlertTypes { get; set; }
        public List<LeadCustomerType> CustomerTypes { get; set; }
        public bool NotifyBefore { get; set; }
        public List<int> FrequencyDaysBefore { get; set; }
        public bool NotifyAfter { get; set; }
        public List<int> FrequencyDaysAfter { get; set; }
        public DateTime? HolidayDate { get; set; }
        public List<AlertConfigurationLanguageModel> Languages { get; set; }
    }
}
