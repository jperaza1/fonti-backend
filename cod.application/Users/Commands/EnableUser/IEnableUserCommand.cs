using System.Threading.Tasks;

namespace cod.application.Users.Commands.EnableUser
{
    public interface IEnableUserCommand
    {
        Task Execute(int id);
    }
}
