using cod.domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.domain.Alerts
{
    [Table("AlertConfiguration", Schema = "alerts")]
    public class AlertConfiguration : EntityTenantable
    {
        public string Name { get; set; }
        public string BodyType { get; set; }
        public string AlertBy { get; set; }
        public string AlertTypes { get; set; }
        public string CustomerTypes { get; set; }
        public bool NotifyBefore { get; set; }
        public string FrequencyDaysBefore { get; set; }
        public bool NotifyAfter { get; set; }
        public string FrequencyDaysAfter { get; set; }
        public DateTime? HolidayDate { get; set; }
        public virtual ICollection<AlertConfigurationLanguage> AlertConfigurationLanguages { get; set; }
    }
}
