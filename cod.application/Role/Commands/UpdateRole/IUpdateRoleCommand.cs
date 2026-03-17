using System.Threading.Tasks;

namespace cod.application.Role.Commands.UpdateRole
{
    public interface IUpdateRoleCommand
    {
        Task Execute(UpdateRoleModel model);
    }
}
