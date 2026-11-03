using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Extensions;
using cod.application.Interfaces;
using cod.domain.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.InvoiceSale.Queries.GetInvoiceReceiptPdf
{
    public class GetInvoiceReceiptPdfQuery : BaseQuery, IGetInvoiceReceiptPdfQuery
    {
        private readonly IPdfCreatorService _pdfCreatorService;
        public GetInvoiceReceiptPdfQuery(IRepository repository, IPdfCreatorService pdfCreatorService) : base(repository)
        {
            _pdfCreatorService = pdfCreatorService;
        }

        public async Task<string> Execute(int invoiceId)
        {
            var invoice = await _repository.Query<domain.Finance.SaleInvoice>()
                .Where(f => f.Id == invoiceId)
                .Select(f => new
                {
                    f.Correlative.Cai,
                    f.Correlative.EndDate,
                    f.Correlative.Start,
                    f.Correlative.End,
                    f.Correlative.EmissionPoint,
                    f.Correlative.Establishment,
                    f.Correlative.DocumentType,
                    f.InvoiceNumber,
                    f.InvoiceDate,
                    f.PaymentType,
                    f.DueDate,
                    CustomerName = f.Customer.Name,
                    CustomerRtn = f.Customer.Rtn,
                    f.CreatorUser,
                    f.Tax18Amount,
                    f.TaxableAmount18,
                    f.TaxableAmount,
                    f.ExemptAmount,
                    f.Discount,
                    f.SubTotal,
                    f.TaxAmount,
                    f.Total,
                    Products = f.Details
                    .SelectMany(d => d.Sale.SaleDetails)
                    .Select(x => new
                    {
                        x.ProductId,
                        ProductName = x.Product.Name,
                        x.Product.Tax,
                        x.Price,
                        x.Quantity,
                        x.Total
                    })
                    .ToList()
                }).FirstOrDefaultAsync();

            var tenant = await _repository.QueryActive<domain.Authorization.ApplicationTenant>()
                .FirstOrDefaultAsync();

            var groupedProducts = invoice?.Products
            .GroupBy(x => new { x.ProductId, x.Price })
            .Select(g => new
            {
                g.Key.ProductId,
                g.Key.Price,
                ProductName = g.First().ProductName,
                Tax = g.First().Tax,
                Quantity = g.Sum(x => x.Quantity),
                Total = g.Sum(x => x.Total)
            })
            .ToList();

            var rpt = new InvoiceReceiptModel
            {
                Logo = tenant.Logo?.ConvertToPictureString(),
                CompanyName = tenant.CompanyName,
                Address = tenant.Address,
                PhoneNumber = tenant.PhoneNumber,
                Email = tenant.Email,
                CompanyRtn = tenant.Rtn,
                Cai = invoice.Cai,
                EmissionLimitDate = invoice.EndDate,
                CorrelativeFrom = $"{invoice.EmissionPoint}-{invoice.Establishment}-{invoice.DocumentType}-{invoice.Start.ToString("D8")}",
                CorrelativeTo = $"{invoice.EmissionPoint}-{invoice.Establishment}-{invoice.DocumentType}-{invoice.End.ToString("D8")}",
                InvoiceNumber = invoice.InvoiceNumber,
                InvoiceDate = invoice.InvoiceDate,
                PaymentTypeName = invoice.PaymentType.GetEnumDescription(),
                DueDate = invoice.DueDate,
                CustomerName = invoice.CustomerName,
                CustomerRtn = invoice.CustomerRtn,
                Advisor = invoice.CreatorUser,
                TaxableAmount = invoice.TaxableAmount,
                TaxableAmount18 = invoice.TaxableAmount18,
                ExemptAmount = invoice.ExemptAmount,
                ExoneratedAmount = 0,
                Discount = invoice.Discount,
                SubTotal = invoice.SubTotal,
                TaxAmount = invoice.TaxAmount,
                Tax18Amount = invoice.Tax18Amount,
                Total = invoice.Total,
                Lines = groupedProducts.Select(x => new InvoiceReceiptLineModel
                {
                    Quantity = x.Quantity,
                    Description = x.ProductName,
                    Tax = x.Tax,
                    UnitPrice = x.Price,
                    Amount = x.Total
                }).ToList(),
            };

            var html = InvoiceReceiptHtml.GetInvoiceReceiptHtml(rpt);
            var pdf = _pdfCreatorService.GetUrlPdf(html, $"Facutra #{invoice.InvoiceNumber}", DinkToPdf.Orientation.Portrait);
            return pdf;

        }
    }
}
