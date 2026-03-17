using System.Threading.Tasks;

namespace cod.application.Configuration.Commands.UpdateConfiguration
{
    public interface IUpdateConfigurationCommand
    {
        Task Execute(UpdateConfigurationModel model, int id);
    }
}