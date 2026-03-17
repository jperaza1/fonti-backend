using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Alerts.Commands.CreateAlertConfiguration
{
    public interface ICreateAlertConfigurationCommand
    {
        Task Execute(CreateAlertConfigurationModel model);
    }
}
