using System.Threading.Tasks;

namespace cod.application.Users.Commands.CreateUser
{
    public interface ICreateUserCommand
    {
        Task Execute(CreateUserModel model);
    }
}
