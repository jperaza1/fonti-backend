using System.Threading.Tasks;

namespace cod.application.Type.Commands.DisableType
{
    public interface IDisableTypeCommand
    {
        Task Execute(int id);
    }
}