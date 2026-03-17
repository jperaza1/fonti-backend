using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cod.application.GroupRole.Queries.GetGroupRolePermissions;
using cod.domain.Authorization;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using cod.application.Features.Base;
using cod.application.Features.Extensions;
using cod.application.Interfaces;

namespace cod.application.GroupRole.Queries.GetPermissionsByCurrentTenant
{
    public class GetPermissionsByCurrentTenantQuery : BaseQuery, IGetPermissionsByCurrentTenantQuery
    {
        private readonly AuthService _authService;

        public GetPermissionsByCurrentTenantQuery(IRepository repository,
            AuthService authService) : base(repository)
        {
            _authService = authService;
        }

        public async Task<List<GetGroupPermissionModel>> Execute()
        {

            List<string> permissions =
                await _repository
                    .Query<ApplicationTenantPermission>()
                    .Where(tenant => tenant.TenantId == _authService.TenantId)
                    .Select(tenant => tenant.PermissionId)
                    .ToListAsync();

            List<GroupRoleDto> permissionsTenant =
                await _repository
                    .QueryActive<ApplicationGroupRole>()
                    .Where(group => group.GroupRolePermissions.Any(permission => permissions.Contains(permission.PermissionId)))
                    .Select(group => new GroupRoleDto
                    {
                        Id = group.Id,
                        Name = group.Name,
                        GroupRoleId = group.GroupHeaderId,
                        GroupRole = group.GroupHeader.Name ?? "",
                        Permissions = group.GroupRolePermissions.Select(permission => new GroupRolePermissionDto
                        {
                            PermissionId = permission.PermissionId,
                            NormalizedName = permission.Role.NormalizedName
                        })
                    })
                    .ToListAsync();

            List<GroupRoleDto> allGroups = await _repository
                .Query<ApplicationGroupRole>()
                .Select(group => new GroupRoleDto
                {
                    Id = group.Id,
                    Name = group.Name,
                    GroupRoleId = group.GroupHeaderId,
                    GroupRole = group.GroupHeader.Name ?? ""
                }).ToListAsync();

            List<GetGroupPermissionModel> tenantPermissions = GetGroupPermissions(permissionsTenant, allGroups, permissions);

            return tenantPermissions;
        }

        public List<GetGroupPermissionModel> GetGroupPermissions(List<GroupRoleDto> groupRoles,
            List<GroupRoleDto> allGroupRoles,
            List<string> permissions)
        {

            if (!groupRoles.Any())
                return new List<GetGroupPermissionModel>();

            var newList = new List<GetGroupPermissionModel>();

            foreach (var groupRole in groupRoles)
            {

                var itemAdded = new GetGroupPermissionModel
                {
                    NormalizedName = groupRole.Name.ToUpper(),
                    DisplayName = groupRole.Name,
                    Parent = groupRole.GroupRole,
                    Children = new List<GetGroupPermissionModel>(),
                    IsChild = !groupRole.GroupRole.NullOrEmpty()
                };

                var groupChild = allGroupRoles.Where(role => role.GroupRoleId == groupRole.Id).ToList();

                if (groupChild.Any())
                    itemAdded.Children.AddRange(this.GetGroupPermissions(groupChild, allGroupRoles, permissions));
                else
                    itemAdded.Children = groupRole.Permissions
                        .Where(permission => permissions.Contains(permission.PermissionId))
                        .Select(permission => new GetGroupPermissionModel
                        {
                            PermissionId = permission.PermissionId,
                            DisplayName = permission.NormalizedName,
                            NormalizedName = permission.NormalizedName,
                            Parent = itemAdded.DisplayName,
                            IsChild = true
                        }).ToList();

                newList.Add(itemAdded);

            }

            return newList;

        }

    }

}