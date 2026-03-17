using System.Threading.Tasks;

namespace cod.application.Configuration.Commands.CreateConfiguration
{
    public interface ICreateConfigurationCommand
    {
        Task Execute(CreateConfigurationModel model);
    }
}