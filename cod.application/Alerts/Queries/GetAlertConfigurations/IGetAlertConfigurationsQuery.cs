using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Alerts.Queries.GetAlertConfigurations
{
    public interface IGetAlertConfigurationsQuery
    {
        Task<List<GetAlertConfigurationsModel>> Execute();
    }
}
