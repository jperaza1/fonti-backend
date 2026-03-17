using System.Threading.Tasks;

namespace cod.application.Configuration.Commands.DisableConfiguration
{
    public interface IDisableConfigurationCommand
    {
        Task Execute(int id);
    }
}