using cod.application.Features.Models;
using cod.application.Invoice.Common;
using cod.application.Payment.Commands.AddPayment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using cod.application.Payment.Common;

namespace cod.data.Controllers
{
    [ApiController]
    public class PaymentController : BaseController
    {
        private readonly IAddPaymentCommand _addPaymentCommand;

        public PaymentController(
            IAddPaymentCommand addPaymentCommand)
        {
            _addPaymentCommand = addPaymentCommand;
        }

        [HttpPost]
        public async Task<IActionResult> AddPayment([FromBody] PaymentModel model)
        {
            try
            {
                await _addPaymentCommand.Execute(model);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }

        }
    }
}
