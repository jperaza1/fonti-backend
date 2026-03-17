using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Alerts.Queries.GetAlertConfigurationById
{
    public interface IGetAlertConfigurationByIdQuery
    {
        Task<GetAlertConfigurationByIdModel> Execute(int id);
    }
}
