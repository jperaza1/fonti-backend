using System.Collections.Generic;
using System.Threading.Tasks;
using cod.application.GroupRole.Queries.GetGroupRolePermissions;

namespace cod.application.GroupRole.Queries.GetPermissionsByCurrentTenant
{
    public interface IGetPermissionsByCurrentTenantQuery
    {
        Task<List<GetGroupPermissionModel>> Execute();
    }
}
