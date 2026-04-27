using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.PurchaseOrder.Queries.GetPurchaseOrder;
using DocuSign.eSign.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.PurchaseOrder.Queries.GetPurchaseOrderPdf
{
    public class GetPurchaseOrderPdfQuery : BaseQuery, IGetPurchaseOrderPdfQuery
    {
        private readonly IGetPurchaseOrderQuery _getPurchaseOrderQuery;
        private readonly IPdfCreatorService _pdfCreatorService;
        public GetPurchaseOrderPdfQuery(
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
            var html = PurchaseOrderHtml.GetPurchaseOrderHtml(purchaseOrder);
            var pdf = _pdfCreatorService.GetUrlPdf(html, $"Orden de compra #{purchaseOrder.Reference}", DinkToPdf.Orientation.Portrait);
            return pdf;
        }
    }
}
