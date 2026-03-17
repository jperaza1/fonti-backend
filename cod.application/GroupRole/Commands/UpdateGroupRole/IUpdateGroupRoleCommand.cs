using System.Threading.Tasks;

namespace cod.application.GroupRole.Commands.UpdateGroupRole
{
    public interface IUpdateGroupRoleCommand
    {
        Task Execute(UpdateGroupRoleModel model);
    }
}
