using System.Linq;
using System.Threading.Tasks;
using cod.domain.Authorization;
using cod.domain.Features;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Features.Extensions;
using cod.application.Interfaces;

namespace cod.application.Role.Commands.CreateRole
{
    public class CreateRoleCommand : BaseCommand, ICreateRoleCommand
    {
        private readonly AuthService _authService;

        public CreateRoleCommand(IRepository repository,
            IExceptionFactory exceptionFactory,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task Execute(CreateRoleModel model)
        {
            await ValidateModel(model);

            ApplicationRol rol = new ApplicationRol
            {
                Name = model.Name,
                Description = model.Description,
                ApplicationRolePermissions = model.Permissions.Select(permission => new ApplicationRolePermission
                {
                    PermissionId = permission,
                    CreationTime = Clock.Now,
                    CreatorUser = _authService.User,
                    IsActive = true,
                    TenantId = _authService.TenantId
                }).ToList()
            };

            _repository.InsertAsync(rol);

            await _repository.CommitAsync();
        }

        public async Task ValidateModel(CreateRoleModel model)
        {
            if (model.Permissions == null || !model.Permissions.Any())
                this._exceptionFactory.Add("permissions", this._exceptionFactory.GetMessage("Permissions", ExceptionEnums.ExceptionCode.Required));
            if (model.Name.NullOrEmpty())
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Name", ExceptionEnums.ExceptionCode.Required));
            if (model.Description.NullOrEmpty())
                this._exceptionFactory.Add("description", this._exceptionFactory.GetMessage("Description", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("TRANSACTION CANCELLED", "ROLE", this._exceptionFactory.Exceptions);

            bool roleExist =
                await _repository
                    .AnyAsync<ApplicationRol>(rol => rol.Name.ToUpper() == model.Name.ToUpper());

            if (roleExist)
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Name", ExceptionEnums.ExceptionCode.EmailDuplicate));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("TRANSACTION CANCELLED", "ROLE", this._exceptionFactory.Exceptions);
        }
    }
}