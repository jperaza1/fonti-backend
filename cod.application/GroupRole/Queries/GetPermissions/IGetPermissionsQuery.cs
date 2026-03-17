using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.GroupRole.Queries.GetPermissions
{
    public interface IGetPermissionsQuery
    {
        Task<List<GetPermissionsModel>> Execute();
    }
}
