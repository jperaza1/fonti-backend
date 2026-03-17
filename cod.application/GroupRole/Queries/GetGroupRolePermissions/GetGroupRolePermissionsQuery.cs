using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LOGISTIC.CONTAINER.APPLICATION.GroupRole.Queries.GetGroupRolePermissions;
using cod.application.GroupRole.Queries.GetPermissionsByCurrentTenant;
using cod.domain.Authorization;
using Microsoft.EntityFrameworkCore;
using cod.application.Features.Base;
using cod.application.Features.Extensions;
using cod.application.Interfaces;

namespace cod.application.GroupRole.Queries.GetGroupRolePermissions
{
    public class GetGroupRolePermissionsQuery : BaseQuery, IGetGroupRolePermissionsQuery
    {
        public GetGroupRolePermissionsQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<GetGroupPermissionModel>> Execute()
        {

            var allGroups = await _repository
                .QueryActive<ApplicationGroupRole>()
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
                }).ToListAsync();

            var groupHeaders = allGroups.Where(c => c.GroupRoleId == null).ToList();

            var permissions = GetGroupPermissions(groupHeaders, allGroups);

            return permissions;
        }

        public List<GetGroupPermissionModel> GetGroupPermissions(List<GroupRoleDto> groupRoles,
            List<GroupRoleDto> allGroupRoles)
        {

            if (!groupRoles.Any())
                return new List<GetGroupPermissionModel>();

            var newList = new List<GetGroupPermissionModel>();

            foreach (var groupRole in groupRoles)
            {

                var groupDefault = allGroupRoles.FirstOrDefault(role => role.Id == groupRole.GroupRoleId);

                var itemAdded = new GetGroupPermissionModel
                {
                    NormalizedName = groupRole.Name.ToUpper(),
                    DisplayName = groupRole.Name,
                    Parent = groupDefault?.Name,
                    Children = new List<GetGroupPermissionModel>(),
                    IsChild = !groupDefault.IsNull()
                };

                var groupChild = allGroupRoles.Where(role => role.GroupRoleId == groupRole.Id).ToList();

                if (groupChild.Any())
                    itemAdded.Children.AddRange(this.GetGroupPermissions(groupChild, allGroupRoles));
                else
                    itemAdded.Children = groupRole.Permissions
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