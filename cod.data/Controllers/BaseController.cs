using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using cod.application.Features.Models;
using ApplicationException =cod.application.Features.Exception.ApplicationException;
using cod.application.Features.Extensions;

namespace cod.data.Controllers
{
    [Route("api/v2/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class BaseController : ControllerBase
    {
        public string ControllerName { get; set; }
        [NonAction]
        public ResponseModel ProcessException(Exception exception)
        {
            if (exception is ApplicationException containerException)
            {
                return new ResponseModel
                {
                    MessageResponse = containerException.MessageResponse,
                    Errors = containerException.Errors,
                    Success = containerException.Success,
                    Title = ControllerName.IsNull() ? containerException.Title : ControllerName
                };
            }
            return new ResponseModel
            {
                Success = false,
                MessageResponse = exception.Message,
                Title = ControllerName
            };
        }
    }
}
