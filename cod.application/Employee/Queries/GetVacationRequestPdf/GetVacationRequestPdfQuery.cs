using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.domain.Operation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Employee.Queries.GetVacationRequestPdf
{
    public class GetVacationRequestPdfQuery : BaseQuery, IGetVacationRequestPdfQuery
    {
        private readonly IPdfCreatorService _pdfCreatorService;
        public GetVacationRequestPdfQuery(
            IRepository repository, IPdfCreatorService pdfCreatorService) : base(repository)
        {
            _pdfCreatorService = pdfCreatorService;
        }

        public async Task<string> Execute(int id)
        {
            var baseQuery = _repository.QueryActive<domain.Rrhh.VacationRequested>()
                .Where(x => x.Id == id)
                .Select(g => new
                {
                    EmployeeName = $"{g.Employee.FirstName} {g.Employee.LastName}",
                    Position = g.Employee.Position.Name,
                    Cafeteria = g.Employee.Cafeteria.Name,
                    g.Employee.StartDate,
                    g.TotalDays,
                    g.Start,
                    g.End,
                    g.CreationTime,
                });

            var request = await baseQuery.Select(g => new GetVacationRequestModel
            {
                Id = id,
                CreationDate = g.CreationTime,
                End = g.End,
                EmployeeName = g.EmployeeName,
                Cafeteria = g.Cafeteria,
                Position = g.Position,
                Start = g.Start,
                StartDate = g.StartDate.Value,
                TotalDays = g.TotalDays,
            }).FirstOrDefaultAsync();

            var html = VacationRequest.GetVacationRequestHtml(request);
            var pdf = _pdfCreatorService.GetUrlPdf(html, $"Solicitud de Vacaciones", DinkToPdf.Orientation.Portrait);
            return pdf;
        }
    }
}
