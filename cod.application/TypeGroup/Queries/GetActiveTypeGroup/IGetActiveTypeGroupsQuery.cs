using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.TypeGroup.Queries.GetActiveTypeGroup
{
    public interface IGetActiveTypeGroupsQuery
    {
        Task<List<GetActiveTypeGroupModel>> Execute();
    }
}