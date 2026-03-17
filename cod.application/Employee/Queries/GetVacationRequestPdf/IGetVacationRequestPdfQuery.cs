using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Employee.Queries.GetVacationRequestPdf
{
    public interface IGetVacationRequestPdfQuery
    {
        Task<string> Execute(int id);
    }
}
