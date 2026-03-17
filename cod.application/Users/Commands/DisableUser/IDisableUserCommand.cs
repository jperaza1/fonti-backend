using System.Threading.Tasks;

namespace cod.application.Users.Commands.DisableUser
{
    public interface IDisableUserCommand
    {
        Task Execute(int id);
    }
}
