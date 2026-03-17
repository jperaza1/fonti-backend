using cod.application.Factory.UserAlertInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Alerts.Queries.GetUserAlerts
{
    public interface IGetUserAlertsQuery
    {
        Task<List<UserAlertInfoModel>> Execute(string user);
    }
}
