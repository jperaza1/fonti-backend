using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.OfficeRequisition.Queries.GetOfficeRequisition;
using cod.application.SpecialService.Queries.GetSpecialService;
using cod.application.SpecialService.Queries.GetSpecialServicePdf;
using cod.domain.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.OfficeRequisition.Queries.GetOfficeRequisitionPdf
{
    public class GetOfficeRequisitionPdfQuery : BaseQuery, IGetOfficeRequisitionPdfQuery
    {
        private readonly IGetOfficeRequisitionQuery _getOfficeRequisitionQuery;
        private readonly IPdfCreatorService _pdfCreatorService;
        public GetOfficeRequisitionPdfQuery(
            IRepository repository, 
            IGetOfficeRequisitionQuery getOfficeRequisitionQuery, 
            IPdfCreatorService pdfCreatorService) : base(repository)
        {
            _getOfficeRequisitionQuery = getOfficeRequisitionQuery;
            _pdfCreatorService = pdfCreatorService;
        }

        public async Task<string> Execute(int id)
        {
            var officeRequisition = await _getOfficeRequisitionQuery.Execute(id);
            var html = OfficeRequisitionHtml.GetOfficeRequisitionHtml(officeRequisition);
            var pdf = _pdfCreatorService.GetUrlPdf(html, $"Orden de Requisición #{officeRequisition.Reference}", DinkToPdf.Orientation.Portrait);
            return pdf;
        }
    }
}
