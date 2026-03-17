using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.PurchaseOrder.Queries.GetPurchaseOrder;
using cod.application.PurchaseOrder.Queries.GetPurchaseOrderPdf;
using cod.application.SpecialService.Queries.GetSpecialService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.SpecialService.Queries.GetSpecialServicePdf
{
    public class GetSpecialServicePdfQuery : BaseQuery, IGetSpecialServicePdfQuery
    {
        private readonly IGetSpecialServiceQuery _getSpecialServiceQuery;
        private readonly IPdfCreatorService _pdfCreatorService;
        public GetSpecialServicePdfQuery(
            IRepository repository,
            IGetSpecialServiceQuery getSpecialServiceQuery,
            IPdfCreatorService pdfCreatorService) : base(repository)
        {
            _getSpecialServiceQuery = getSpecialServiceQuery;
            _pdfCreatorService = pdfCreatorService;
        }

        public async Task<string> Execute(int id)
        {
            var specialService = await _getSpecialServiceQuery.Execute(id);
            var html = SpecialServiceHtml.GetSpecialServiceHtml(specialService);
            var pdf = _pdfCreatorService.GetUrlPdf(html, $"Servicio Especial #{specialService.Reference}", DinkToPdf.Orientation.Portrait);
            return pdf;
        }
    }
}
