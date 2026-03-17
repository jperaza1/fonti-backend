using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Alerts.Commands.EnableAlertConfiguration
{
    public interface IEnableAlertConfigurationCommand
    {
        Task Execute(int id);
    }
}
