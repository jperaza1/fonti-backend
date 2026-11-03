using cod.application.Features.Models;
using cod.application.Invoice.Commands.ChangePaymentType;
using cod.application.Invoice.Commands.DeleteInvoice;
using cod.application.Invoice.Commands.UpdateDocumentId;
using cod.application.Invoice.Commands.UpdateInvoice;
using cod.application.Invoice.Commands.UpdateInvoiceFile;
using cod.application.Invoice.Common;
using cod.application.Invoice.Queries.GetInvoice;
using cod.application.Invoice.Queries.GetInvoices;
using cod.application.Invoice.Queries.GetPendingInvoices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
    [ApiController]
    public class InvoiceController : BaseController
    {
        private readonly IGetInvoiceQuery _getInvoiceQuery;
        private readonly IGetInvoicesQuery _getInvoicesQuery;
        private readonly IGetPendingInvoicesQuery _getPendingInvoicesQuery;
        private readonly IUpdateDocumentIdCommand _updateDocumentIdCommand;
        private readonly IUpdateInvoiceFileCommand _updateInvoiceFileCommand;
        private readonly IUpdateInvoiceCommand _updateInvoiceCommand;
        private readonly IChangePaymentTypeCommand _changePaymentTypeCommand;
        private readonly IDeleteInvoiceCommand _deleteInvoiceCommand;
        public InvoiceController(
            IGetInvoiceQuery invoiceQuery,
            IGetInvoicesQuery getInvoicesQuery,
            IGetPendingInvoicesQuery getPendingInvoicesQuery,
            IUpdateDocumentIdCommand updateDocumentIdCommand,
            IUpdateInvoiceFileCommand updateInvoiceFileCommand,
            IUpdateInvoiceCommand updateInvoiceCommand,
            IChangePaymentTypeCommand changePaymentTypeCommand,
            IDeleteInvoiceCommand deleteInvoiceCommand)
        {
            _getInvoiceQuery = invoiceQuery;
            _getInvoicesQuery = getInvoicesQuery;
            _getPendingInvoicesQuery = getPendingInvoicesQuery;
            _updateDocumentIdCommand = updateDocumentIdCommand;
            _updateInvoiceFileCommand = updateInvoiceFileCommand;
            _updateInvoiceCommand = updateInvoiceCommand;
            _changePaymentTypeCommand = changePaymentTypeCommand;
            _deleteInvoiceCommand = deleteInvoiceCommand;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInvoiceFile(int id, [FromBody] bool status)
        {
            try
            {
                await _updateInvoiceFileCommand.Execute(id, status);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }

        }

        [HttpPost]
        public async Task<List<InvoiceModel>> GetAll([FromBody] FilterInvoiceModel filter)
        {
            return await _getInvoicesQuery.Execute(filter);
        }

        [HttpPost]
        public async Task<List<InvoiceModel>> GetPendingPaid([FromBody] GetPendingInvoicesModel filter)
        {
            return await _getPendingInvoicesQuery.Execute(filter);
        }

        [HttpGet]
        public async Task<InvoiceModel> GetById(int id)
        {
            return await _getInvoiceQuery.Execute(id);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateDocumentId([FromBody] UpdateDocumentIdModel model)
        {
            try
            {
                await _updateDocumentIdCommand.Execute(model);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }

        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] InvoiceModel model, int id)
        {
            try
            {
                await _updateInvoiceCommand.Execute(id, model);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }

        }

        [HttpPut]
        public async Task<IActionResult> ChangePaymentType(int id)
        {
            try
            {
                await _changePaymentTypeCommand.Execute(id);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _deleteInvoiceCommand.Execute(id);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }

        }
    }
}
