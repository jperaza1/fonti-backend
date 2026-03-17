using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using cod.application.GroupRole.Queries.GetGroupRolePermissions;

namespace LOGISTIC.CONTAINER.APPLICATION.GroupRole.Queries.GetGroupRolePermissions
{
    public interface IGetGroupRolePermissionsQuery
    {
        Task<List<GetGroupPermissionModel>> Execute();
    }
}
