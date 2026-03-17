using System.Threading.Tasks;

namespace cod.application.Type.Commands.UpdateType
{
    public interface IUpdateTypeCommand
    {
        Task Execute(UpdateTypeModel model, int id);
    }
}