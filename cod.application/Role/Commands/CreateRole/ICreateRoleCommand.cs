using System.Threading.Tasks;

namespace cod.application.Role.Commands.CreateRole
{
    public interface ICreateRoleCommand
    {
        Task Execute(CreateRoleModel model);
    }
}
