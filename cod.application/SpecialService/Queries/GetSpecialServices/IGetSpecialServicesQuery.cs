using cod.application.SpecialService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.SpecialService.Queries.GetSpecialServices
{
    public interface IGetSpecialServicesQuery
    {
        Task<List<SpecialServiceModel>> Execute(FilterSpecialService filterModel);
    }
}
