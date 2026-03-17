using cod.application.OfficeRequisition.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.OfficeRequisition.Queries.GetOfficeRequisition
{
    public interface IGetOfficeRequisitionQuery
    {
        Task<OfficeRequisitionModel> Execute(int id);
    }
}
