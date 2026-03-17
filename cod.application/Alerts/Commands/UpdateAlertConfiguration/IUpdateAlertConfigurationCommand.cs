using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Alerts.Commands.UpdateAlertConfiguration
{
    public interface IUpdateAlertConfigurationCommand
    {
        Task Execute(UpdateAlertConfigurationModel model, int id);
    }
}
