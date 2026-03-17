using cod.application.SpecialService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.SpecialService.Queries.GetSpecialService
{
    public interface IGetSpecialServiceQuery
    {
        Task<SpecialServiceModel> Execute(int id);
    }
}
