using System.Linq;
using System.Threading.Tasks;
using cod.domain.Authorization;
using cod.domain.Features;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;

namespace cod.application.GroupRole.Commands.UpdateGroupRole
{
    public class UpdateGroupRoleCommand : BaseCommand, IUpdateGroupRoleCommand
    {
        private readonly AuthService _authService;

        public UpdateGroupRoleCommand(IRepository repository,
            IExceptionFactory exceptionFactory,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task Execute(UpdateGroupRoleModel model)
        {

            ApplicationGroupRole groupRole =
                await _repository
                    .FirstOrDefaultAsync<ApplicationGroupRole>(group => group.Id == model.Id);

            groupRole.Name = model.Name;
            groupRole.GroupHeaderId = model.GroupHeaderId;

            var permissions = groupRole.GroupRolePermissions;

            var toAdd = model.Permissions
                 .Where(permission => permissions.All(db => db.PermissionId != permission.PermissionId))
                 .ToList();

            var toRemove = permissions
                .Where(pm => model.Permissions.All(permission => permission.PermissionId != pm.PermissionId))
                .ToList();

            foreach (var permission in toAdd)
            {
                _repository.InsertAsync(new ApplicationGroupRolePermission
                {
                    PermissionId = permission.PermissionId,
                    GroupRoleId = model.Id
                });
            }

            foreach (var permission in toRemove)
            {
                _repository.DeleteSync(permission);
            }

            _repository.UpdateSync(groupRole);

            await _repository.CommitAsync();

        }
    }
}