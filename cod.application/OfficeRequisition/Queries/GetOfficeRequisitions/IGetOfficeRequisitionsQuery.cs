using cod.application.OfficeRequisition.Common;
using cod.application.SpecialService.Common;
using cod.application.SpecialService.Queries.GetSpecialServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.OfficeRequisition.Queries.GetOfficeRequisitions
{
    public interface IGetOfficeRequisitionsQuery
    {
        Task<List<OfficeRequisitionModel>> Execute(FilterOfficeRequisition filterModel);
    }
}
