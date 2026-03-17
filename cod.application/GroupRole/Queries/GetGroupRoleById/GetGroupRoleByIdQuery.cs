using System.Linq;
using System.Threading.Tasks;
using cod.domain.Authorization;
using cod.application.Features.Base;
using cod.application.GroupRole.Commands.CreateGroupRole;
using cod.application.Interfaces;

namespace cod.application.GroupRole.Queries.GetGroupRoleById
{
    public class GetGroupRoleByIdQuery : BaseQuery, IGetGroupRoleByIdQuery
    {
        public async Task<GetGroupRoleByIdModel> Execute(int id)
        {
            ApplicationGroupRole groupRole =
                await _repository
                    .FirstOrDefaultActive<ApplicationGroupRole>(group => group.Id == id);

            return new GetGroupRoleByIdModel
            {
                Id = groupRole.Id,
                Name = groupRole.Name,
                GroupHeaderId = groupRole.GroupHeaderId,
                Permissions = groupRole
                    .GroupRolePermissions.
                    Select(permission => new CreateGroupRolePermissionModel
                    {
                        PermissionId = permission.PermissionId,
                        NormalizedName = permission.Role.NormalizedName
                    }).ToList()
            };
        }

        public GetGroupRoleByIdQuery(IRepository repository) : base(repository)
        {
        }
    }
}