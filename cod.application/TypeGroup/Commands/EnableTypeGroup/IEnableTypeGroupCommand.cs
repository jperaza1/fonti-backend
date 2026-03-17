using System.Threading.Tasks;

namespace cod.application.TypeGroup.Commands.EnableTypeGroup
{
   public interface IEnableTypeGroupCommand
    {
        Task Execute(int id);
    }
}
