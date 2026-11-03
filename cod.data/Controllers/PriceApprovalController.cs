using cod.application.Features.Models;
using cod.application.PriceApproval.Commands.ApproveRequest;
using cod.application.PriceApproval.Common;
using cod.application.PriceApproval.Queries.GetRequestByToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class PriceApprovalController : BaseController
    {
        private readonly IGetRequestByTokenQuery _getRequestByTokenQuery;
        private readonly IApproveRequestCommand _approveRequestCommand;

        public PriceApprovalController(
            IGetRequestByTokenQuery getRequestByTokenQuery,
            IApproveRequestCommand approveRequestCommand)
        {
            _getRequestByTokenQuery = getRequestByTokenQuery;
            _approveRequestCommand = approveRequestCommand;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<PriceApprovalModel> GetByToken(string token)
        {
            return await _getRequestByTokenQuery.Execute(token);
        }

        [HttpPost]
        public async Task<IActionResult> Resolve([FromBody] ApproveRequestModel model)
        {
            try
            {
                await _approveRequestCommand.Execute(model);
                return Ok(new ResponseModel { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }
    }
}
