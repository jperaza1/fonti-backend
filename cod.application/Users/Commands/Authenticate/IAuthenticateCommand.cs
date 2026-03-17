using System.Threading.Tasks;

namespace cod.application.Users.Commands.Authenticate
{
    public interface IAuthenticateCommand
    {
        Task<AuthenticateResponseModel> Execute(AuthenticateRequestModel authenticateRequestModel);
    }
}
