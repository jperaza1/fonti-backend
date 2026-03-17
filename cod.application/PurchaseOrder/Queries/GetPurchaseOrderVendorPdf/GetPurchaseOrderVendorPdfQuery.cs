using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.PurchaseOrder.Queries.GetPurchaseOrder;
using cod.application.PurchaseOrder.Queries.GetPurchaseOrderPdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.PurchaseOrder.Queries.GetPurchaseOrderVendorPdf
{
    public class GetPurchaseOrderVendorPdfQuery : BaseQuery, IGetPurchaseOrderVendorPdfQuery
    {
        private readonly IGetPurchaseOrderQuery _getPurchaseOrderQuery;
        private readonly IPdfCreatorService _pdfCreatorService;
        public GetPurchaseOrderVendorPdfQuery(
            IRepository repository,
            IGetPurchaseOrderQuery getPurchaseOrderQuery,
            IPdfCreatorService pdfCreatorService) : base(repository)
        {
            _getPurchaseOrderQuery = getPurchaseOrderQuery;
            _pdfCreatorService = pdfCreatorService;
        }

        public async Task<string> Execute(int id)
        {
            var purchaseOrder = await _getPurchaseOrderQuery.Execute(id);
            var html = PurchaseOrderVendorHtml.GetPurchaseOrderVendorHtml(purchaseOrder);
            var pdf = _pdfCreatorService.GetUrlPdf(html, $"Orden de compra #{purchaseOrder.Reference}", DinkToPdf.Orientation.Portrait);
            return pdf;
        }
    }
}
