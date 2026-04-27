using cod.application.Features.Models;
using cod.application.Invoice.Commands.CreateInvoice;
using cod.application.Invoice.Commands.UpdateDocumentId;
using cod.application.Invoice.Commands.UpdateInvoiceFile;
using cod.application.Invoice.Common;
using cod.application.Invoice.Queries.GetInvoice;
using cod.application.Invoice.Queries.GetInvoices;
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
        private readonly ICreateInvoiceCommand _createInvoiceCommand;
        private readonly IUpdateDocumentIdCommand _updateDocumentIdCommand;
        private readonly IUpdateInvoiceFileCommand _updateInvoiceFileCommand;
        public InvoiceController(
            IGetInvoiceQuery invoiceQuery,
            IGetInvoicesQuery getInvoicesQuery,
            ICreateInvoiceCommand createInvoiceCommand,
            IUpdateDocumentIdCommand updateDocumentIdCommand,
            IUpdateInvoiceFileCommand updateInvoiceFileCommand)
        {
            _getInvoiceQuery = invoiceQuery;
            _getInvoicesQuery = getInvoicesQuery;
            _createInvoiceCommand = createInvoiceCommand;
            _updateDocumentIdCommand = updateDocumentIdCommand;
            _updateInvoiceFileCommand = updateInvoiceFileCommand;
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

        [HttpGet]
        public async Task<InvoiceModel> GetById(int id)
        {
            return await _getInvoiceQuery.Execute(id);
        }

        [HttpPost]
        public async Task<IActionResult> AddInvoice([FromBody] InvoiceModel model)
        {
            try
            {
                await _createInvoiceCommand.Execute(model);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }

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
    }
}
