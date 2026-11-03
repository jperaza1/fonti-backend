using cod.application.Features.Models;
using cod.application.Invoice.Common;
using cod.application.Payment.Commands.AddPayment;
using cod.application.Payment.Commands.CreatePayment;
using cod.application.Payment.Commands.DeletePayment;
using cod.application.Payment.Commands.FinishPayment;
using cod.application.Payment.Commands.GenerateFilePayment;
using cod.application.Payment.Commands.UpdatePayment;
using cod.application.Payment.Commands.UpdatePaymentNote;
using cod.application.Payment.Common;
using cod.application.Payment.Queries.GetAllPayment;
using cod.application.Payment.Queries.GetPaymentById;
using cod.application.Payment.Queries.GetPaymentReceiptPdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class PaymentController : BaseController
    {
        private readonly IGetAllPaymentQuery _getAllPayment;
        private readonly IGetPaymentByIdQuery _getPaymentByIdQuery;
        private readonly IGetPaymentReceiptPdfQuery _getPaymentReceiptPdfQuery;
        private readonly IAddPaymentCommand _addPaymentCommand;
        private readonly ICreatePaymentCommand _createPaymentCommand;
        private readonly IGenerateFilePaymentCommand _generateFilePaymentCommand;
        private readonly IFinishPaymentCommand _finishPaymentCommand;
        private readonly IUpdatePaymentCommand _updatePaymentCommand;
        private readonly IDeletePaymentCommand _deletePaymentCommand;
        private readonly IUpdatePaymentNoteCommand _updatePaymentNoteCommand;
        public PaymentController(
            IGetAllPaymentQuery getAllPayment,
            IGetPaymentByIdQuery getPaymentById,
            IGetPaymentReceiptPdfQuery getPaymentReceiptPdfQuery,
            IAddPaymentCommand addPaymentCommand,
            ICreatePaymentCommand createPaymentCommand,
            IGenerateFilePaymentCommand generateFilePaymentCommand,
            IFinishPaymentCommand finishPaymentCommand,
            IUpdatePaymentCommand updatePaymentCommand,
            IDeletePaymentCommand deletePaymentCommand,
            IUpdatePaymentNoteCommand updatePaymentNoteCommand)
        {
            _getAllPayment = getAllPayment;
            _getPaymentByIdQuery = getPaymentById;
            _getPaymentReceiptPdfQuery = getPaymentReceiptPdfQuery;
            _addPaymentCommand = addPaymentCommand;
            _createPaymentCommand = createPaymentCommand;
            _generateFilePaymentCommand = generateFilePaymentCommand;
            _finishPaymentCommand = finishPaymentCommand;
            _updatePaymentCommand = updatePaymentCommand;
            _deletePaymentCommand = deletePaymentCommand;
            _updatePaymentNoteCommand = updatePaymentNoteCommand;
        }

        [HttpPost]
        public async Task<List<PaymentModel>> GetAll([FromBody] FilterPaymentModel model)
        {
            return await _getAllPayment.Execute(model);
        }

        [HttpGet]
        public async Task<PaymentModel> GetById(int id)
        {
            return await _getPaymentByIdQuery.Execute(id);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentModel model)
        {
            try
            {
                var result = await _createPaymentCommand.Execute(model);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName, Data = result });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }

        }

        [HttpGet]
        public async Task<string> GetPaymentReceiptFile(int paymentId, int vendorId)
        {
            return await _getPaymentReceiptPdfQuery.Execute(paymentId, vendorId);
        }

        [HttpPut]
        public async Task<IActionResult> Update(int id, [FromBody] PaymentModel model)
        {
            try
            {
                await _updatePaymentCommand.Execute(id, model);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }

        }

        [HttpPost]
        public async Task<IActionResult> FinishPayment(int id, [FromBody] PaymentModel model)
        {
            try
            {
                await _finishPaymentCommand.Execute(id, model);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }

        }

        [HttpPost]
        public async Task<IActionResult> UpdateNote(int id, [FromBody] PaymentModel model)
        {
            try
            {
                await _updatePaymentNoteCommand.Execute(id, model);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPost]
        public async Task<IActionResult> GenerateFilePayment(int id)
        {
            try
            {
                var response = await _generateFilePaymentCommand.Execute(id);
                return Ok(new
                {
                    FileName = response.FileName,
                    File = Convert.ToBase64String(response.File) // o el span/byte[] que corresponda
                });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPayment([FromBody] InvoicePaymentModel model)
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

        [HttpDelete]
        public async Task<IActionResult> DeletePayment(int id)
        {
            try
            {
                await _deletePaymentCommand.Execute(id);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }

        }
    }
}
