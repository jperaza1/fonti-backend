using cod.application.DispatchControl.Commom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.DispatchControl.Queries.GenerateReportDispatchControl
{
    public interface IGenerateReportDispatchControlQuery
    {
        Task<byte[]> Execute(DispatchControlFilterModel filterModel);
    }
}
