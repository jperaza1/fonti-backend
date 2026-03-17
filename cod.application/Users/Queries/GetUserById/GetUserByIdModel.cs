using cod.application.Users.Commands.UpdateUser;

namespace cod.application.Users.Queries.GetUserById
{
    public class GetUserByIdModel : UpdateUserModel
    {
        public bool IsActive { get; set; }
    }
}