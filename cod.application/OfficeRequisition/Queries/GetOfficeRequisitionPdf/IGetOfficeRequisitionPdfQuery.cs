using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.OfficeRequisition.Queries.GetOfficeRequisitionPdf
{
    public interface IGetOfficeRequisitionPdfQuery
    {
        Task<string> Execute(int id);
    }
}
