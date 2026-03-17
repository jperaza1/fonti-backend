using System.Threading.Tasks;

namespace cod.application.GroupRole.Commands.CreateGroupRole
{
    public interface ICreateGroupRoleCommand
    {
        Task Execute(CreateGroupRoleModel model);
    }
}
