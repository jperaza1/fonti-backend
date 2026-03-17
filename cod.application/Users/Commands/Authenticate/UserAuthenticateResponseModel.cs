using System;
using System.Collections.Generic;
using cod.domain.Features.Enums;

namespace cod.application.Users.Commands.Authenticate
{
    public class UserAuthenticateResponseModel
    {
        public string Identity { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TenantName { get; set; }
        public int? TenantId { get; set; }
        public int? CafeteriaId { get; set; }
        public DomainEnums.UserType UserType { get; set; }
        public DateTime? Expiration { get; set; }
        public IList<string> Permissions { get; set; }
    }
}
