using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.Role.Queries.GetRoles
{
    public interface IGetRolesQuery
    {
        Task<List<GetRolesModel>> Execute();
    }
}
