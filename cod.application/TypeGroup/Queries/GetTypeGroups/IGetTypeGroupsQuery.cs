using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.TypeGroup.Queries.GetTypeGroups
{
    public interface IGetTypeGroupsQuery
    {
        Task<List<GetTypeGroupsModel>> Execute();
    }
}
