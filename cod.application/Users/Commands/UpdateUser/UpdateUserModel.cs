using cod.application.Users.Commands.CreateUser;

namespace cod.application.Users.Commands.UpdateUser
{
    public class UpdateUserModel : CreateUserModel
    {
        public int Id { get; set; }
        public bool ChangeUsername { get; set; }
        public string NewUserName { get; set; }
        public bool ChangePassword { get; set; }
        public string NewPassword { get; set; }
    }
}