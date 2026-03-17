using System.Threading.Tasks;

namespace cod.application.Users.Commands.UpdateUser
{
    public interface IUpdateUserCommand
    {
        Task Execute(UpdateUserModel model, int id);
    }
}
