using cod.application.Features.Models;
using cod.application.Invoice.Queries.GetInvoices;
using cod.application.InvoiceSale.Commands.GenerateInvoiceSale;
using cod.application.InvoiceSale.Commands.MarkPaid;
using cod.application.InvoiceSale.Common;
using cod.application.InvoiceSale.Queries.GetAllInvoiceSale;
using cod.application.InvoiceSale.Queries.GetInvoiceReceiptPdf;
using cod.application.InvoiceSale.Queries.GetInvoiceSaleById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.data.Controllers
{
     
    [ApiController]
    public class InvoiceSaleController : BaseController
    {
        private readonly IGetAllInvoiceSaleQuery _getAllInvoiceSaleQuery;
        private readonly IGetInvoiceSaleByIdQuery _getInvoiceSaleByIdQuery;
        private readonly IGetInvoiceReceiptPdfQuery _getInvoiceReceiptPdfQuery;
        private readonly IGenerateInvoiceSaleCommand _generateInvoiceSaleCommand;
        private readonly IMarkPaidCommand _markPaidCommand;

        public InvoiceSaleController(
            IGetInvoiceReceiptPdfQuery getInvoiceReceiptPdfQuery,
            IGetInvoiceSaleByIdQuery getInvoiceSaleByIdQuery,
            IGenerateInvoiceSaleCommand generateInvoiceSaleCommand,
            IMarkPaidCommand markPaidCommand,
            IGetAllInvoiceSaleQuery getAllInvoiceSaleQuery)
        {
            _getInvoiceSaleByIdQuery = getInvoiceSaleByIdQuery;
            _getInvoiceReceiptPdfQuery = getInvoiceReceiptPdfQuery;
            _getAllInvoiceSaleQuery = getAllInvoiceSaleQuery;
            _generateInvoiceSaleCommand = generateInvoiceSaleCommand;
            _markPaidCommand = markPaidCommand;
        }

        [HttpGet]
        public async Task<SaleInvoiceModel> GetById(int id)
        {
            return await _getInvoiceSaleByIdQuery.Execute(id);
        }

        [HttpPost]
        public async Task<List<SaleInvoiceModel>> GetAll([FromBody] FilterInvoiceSaleModel filter)
        {
            return await _getAllInvoiceSaleQuery.Execute(filter);
        }

        [HttpGet]
        public async Task<string> GetInvoiceFile(int invoiceId)
        {
            return await _getInvoiceReceiptPdfQuery.Execute(invoiceId);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSaleInvoice([FromBody] GeneratePendingBillingModel model)
        {
            try
            {
                await _generateInvoiceSaleCommand.Execute(model);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }

        [HttpPut]
        public async Task<IActionResult> MarkAsPaid(int id)
        {
            try
            {
                await _markPaidCommand.Execute(id);
                return Ok(new ResponseModel<int> { Success = true, MessageResponse = "SUCCESS", Title = ControllerName });
            }
            catch (Exception exception)
            {
                return Ok(ProcessException(exception));
            }
        }
    }
}
