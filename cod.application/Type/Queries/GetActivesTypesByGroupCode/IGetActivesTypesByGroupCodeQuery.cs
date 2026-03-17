using cod.application.Type.Queries.GetTypesByGroupCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Type.Queries.GetActivesTypesByGroupCode
{
    public interface IGetActivesTypesByGroupCodeQuery
    {
        Task<List<GetTypesByGroupCodeModel>> Execute(string groupCode);
    }
}
