using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Alerts.Queries.GetUserAlertsData
{
    public interface IGetUserAlertsDataQuery
    {
        Task<List<UserAlertDataModel>> Execute(string user);
    }
}
