using System;

namespace cod.application.Users.Commands.Authenticate
{
    public class AuthenticateResponseModel
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public UserAuthenticateResponseModel UserAuth { get; set; }
    }
}
