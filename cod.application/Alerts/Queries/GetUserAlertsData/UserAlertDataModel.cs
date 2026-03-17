using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Alerts.Queries.GetUserAlertsData
{
    public class UserAlertDataModel
    {
        public DomainEnums.UserAlertOption UserAlert { get; set; }
        public string UserAlertName { get; set; }
        public int Count { get; set; }
    }
}
