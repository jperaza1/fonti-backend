using System.Threading.Tasks;

namespace cod.application.TypeGroup.Commands.DisableTypeGroup
{
    public interface IDisableTypeGroupCommand
    {
        Task Execute(int id);
    }
}
