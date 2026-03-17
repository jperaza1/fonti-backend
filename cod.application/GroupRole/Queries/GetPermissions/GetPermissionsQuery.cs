using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace cod.application.GroupRole.Queries.GetPermissions
{
    public class GetPermissionsQuery : IGetPermissionsQuery
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        public GetPermissionsQuery(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<List<GetPermissionsModel>> Execute()
        {
            return await _roleManager
                .Roles
                .Select(permission => new GetPermissionsModel
                {
                    PermissionId = permission.Id,
                    Name = permission.Name,
                    NormalizedName = permission.NormalizedName
                }).ToListAsync();
        }
    }
}