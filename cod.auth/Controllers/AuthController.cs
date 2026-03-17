using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using cod.application.Users.Commands.Authenticate;

namespace cod.auth.Controllers
{
    [Route("api/v2/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticateCommand _authenticateCommand;
        public AuthController(
            IAuthenticateCommand authenticateCommand
        )
        {
            _authenticateCommand = authenticateCommand; 
        }

        [HttpPost]
        [ActionName("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequestModel authenticateRequestModel)
        {
            return Ok(await _authenticateCommand.Execute(authenticateRequestModel));
        }
    }
}
