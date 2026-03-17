using System.Linq;
using System.Threading.Tasks;
using cod.domain.Authorization;
using cod.domain.Features;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;

namespace cod.application.GroupRole.Commands.CreateGroupRole
{
    public class CreateGroupRoleCommand : BaseCommand, ICreateGroupRoleCommand
    {
        private readonly AuthService _authService;

        public CreateGroupRoleCommand(IRepository repository,
            IExceptionFactory exceptionFactory,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task Execute(CreateGroupRoleModel model)
        {

            ApplicationGroupRole groupRole = new ApplicationGroupRole
            {
                Name = model.Name,
                GroupHeaderId = model.GroupHeaderId,
                GroupRolePermissions = model
                    .Permissions
                    .Select(permission => new ApplicationGroupRolePermission
                    {
                        PermissionId = permission.PermissionId,
                        IsActive = true,
                        CreationTime = Clock.Now,
                        CreatorUser = _authService.User
                    }).ToList()
            };

            _repository.InsertAsync(groupRole);

            await _repository.CommitAsync();
        }

    }
}