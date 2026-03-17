using System.Threading.Tasks;

namespace cod.application.TypeGroup.Commands.CreateTypeGroup
{
    public interface ICreateTypeGroupCommand
    {
        Task Execute(CreateTypeGroupModel createModel);
    }
}
