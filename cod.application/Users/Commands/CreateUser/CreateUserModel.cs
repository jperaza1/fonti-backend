using System.Collections.Generic;
using cod.domain.Features.Enums;

namespace cod.application.Users.Commands.CreateUser
{
    public class CreateUserModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        
        public int? CafeteriaId { get; set; }
        public DomainEnums.UserType UserType { get; set; }
        public List<int> RoleIds { get; set; }
        public List<DomainEnums.UserAlertOption> UserAlerts { get; set; }
    }
}