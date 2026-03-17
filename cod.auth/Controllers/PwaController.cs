using System;
using System.Threading.Tasks;
using Lib.Net.Http.WebPush;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using cod.application.Features.Models;
using cod.application.Features.Models.PushService;
using cod.application.Interfaces;

namespace cod.auth.Controllers
{
    [ApiController]
    [Authorize]
    public class PwaController : BaseController
    {
        private readonly IPushService _pushService;

        public PwaController(IPushService pushService)
        {
            _pushService = pushService;
            this.ControllerName = "Pwa";
        }

        [HttpGet]
        public async Task<VapidPublicKeyModel> GetPublicKey()
        {
            return await _pushService.GetPublicKey();
        }

        [HttpPost]
        public async Task<IActionResult> InsertSubscription(PushSubscription pushSubscription)
        {
            try
            {
                await _pushService.InsertUserSubscription(pushSubscription);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = this.ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(this.ProcessException(exception));
            }
        }
    }
}
