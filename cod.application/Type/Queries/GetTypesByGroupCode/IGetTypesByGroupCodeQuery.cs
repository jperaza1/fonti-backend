using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.Type.Queries.GetTypesByGroupCode
{
    public interface IGetTypesByGroupCodeQuery
    {
        Task<List<GetTypesByGroupCodeModel>> Execute(string groupCode);
    }
}
