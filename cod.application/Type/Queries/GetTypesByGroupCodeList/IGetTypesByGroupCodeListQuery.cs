using System.Collections.Generic;
using System.Threading.Tasks;
using cod.application.Type.Queries.GetTypesByGroupCode;

namespace cod.application.Type.Queries.GetTypesByGroupCodeList
{
    public interface IGetTypesByGroupCodeListQuery
    {
        Task<Dictionary<string, List<GetTypesByGroupCodeModel>>> Execute(List<GetTypesByGroupCodeListModel> codes);
    }
}
