using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Payment.Common;
using cod.application.Payment.Queries.GetPaymentById;
using cod.domain.Operation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Payment.Queries.GetPaymentReceiptPdf
{
    public class GetPaymentReceiptPdfQuery : BaseQuery, IGetPaymentReceiptPdfQuery
    {
        private readonly IPdfCreatorService _pdfCreatorService;
        private readonly IGetPaymentByIdQuery _getPaymentByIdQuery;
        public GetPaymentReceiptPdfQuery(IRepository repository, 
            IPdfCreatorService pdfCreatorService,
            IGetPaymentByIdQuery getPaymentByIdQuery) : base(repository)
        {
            _pdfCreatorService = pdfCreatorService;
            _getPaymentByIdQuery = getPaymentByIdQuery;
        }

        public async Task<string> Execute(int paymentId, int vendorId)
        {
            var payment = await _getPaymentByIdQuery.Execute(paymentId);
            payment.Details = payment.Details.Where(f => f.VendorId == vendorId).ToList();

            var vendor = await _repository.Query<domain.Data.Vendor>()
                .Where(f => f.Id == vendorId).FirstOrDefaultAsync();


            var firstDetail = payment.Details.FirstOrDefault();

            var paymentReceipt = new PaymentReceiptModel
            {
                Id = payment.Id,
                Vendor = firstDetail?.BankAccountHolder,
                BankName = firstDetail?.BankName,
                BankAccount = firstDetail?.BankAccount,
                DestinationBranch = firstDetail?.DestinationBranch,
                DestinationOffice = firstDetail?.DestinationOffice,
                BankReference = payment.BankReference,
                Date = payment.PaymentDate,
                Reference = payment.Reference,
                Rtn = vendor.IdentificationHolder,
                Total = payment.Details.Sum(f => f.Amount),
                Details = payment.Details.Select(f => new PaymentReceiptDetailModel
                {
                    InvoiceDate = f.InvoiceDate,
                    InvoiceNumber = f.InvoiceNumber,
                    Total = f.Amount,
                }).ToList()
            };

            var html = PaymentReceiptHtml.GetaymentReceiptHtml(paymentReceipt);
            var pdf = _pdfCreatorService.GetUrlPdf(html, $"Comprobante de Pago #{paymentReceipt.Reference} - {paymentReceipt.Vendor}", DinkToPdf.Orientation.Portrait);
            return pdf;
        }
    }
}
