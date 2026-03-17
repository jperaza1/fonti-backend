using System.Threading.Tasks;

namespace cod.application.Configuration.Commands.EnableConfiguration
{
    public interface IEnableConfigurationCommand
    {
        Task Execute(int id);
    }
}