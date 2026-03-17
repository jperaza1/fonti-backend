using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.GroupRole.Queries.GetGroupRoles
{
    public interface IGetGroupRolesQuery
    {
        Task<List<GetGroupRolesModel>> Execute();
    }
}
