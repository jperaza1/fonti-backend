using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.SpecialService.Queries.GetSpecialServicePdf
{
    public interface IGetSpecialServicePdfQuery
    {
        Task<string> Execute(int id);
    }
}
