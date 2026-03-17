using System.Threading.Tasks;

namespace cod.application.TypeGroup.Commands.UpdateTypeGroup
{
    public interface IUpdateTypeGroupCommand
    {
        Task Execute(UpdateTypeGroupModel model, int id);
    }
}
