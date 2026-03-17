using System.Threading.Tasks;

namespace cod.application.Type.Commands.EnableType
{
    public interface IEnableTypeCommand
    {
        Task Execute(int id);
    }
}