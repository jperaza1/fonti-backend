using System.Threading.Tasks;

namespace cod.application.Type.Commands.CreateType
{
    public interface ICreateTypeCommand
    {
        Task Execute(CreateTypeModel model);
    }
}