using System.Linq;
using System.Threading.Tasks;
using cod.domain.Authorization;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Features.Extensions;
using cod.application.Interfaces;

namespace cod.application.Role.Commands.UpdateRole
{
    public class UpdateRoleCommand : BaseCommand, IUpdateRoleCommand
    {
        private readonly AuthService _authService;

        public UpdateRoleCommand(IRepository repository,
            IExceptionFactory exceptionFactory,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task Execute(UpdateRoleModel model)
        {
            await ValidateModel(model);


            await _repository.UpdateAsync<ApplicationRol>(tenant => tenant.Id == model.Id,
                newValuesTenants => new ApplicationRol
                {
                    Name = model.Name,
                    Description = model.Description
                });

            var permissions = await _repository
                .Query<ApplicationRolePermission>()
                .Where(rol => rol.RolId == model.Id).ToListAsync();

            var toAdd = model.Permissions
                .Where(permission => permissions.All(db => db.PermissionId != permission))
                .ToList();

            var toRemove = permissions
                .Where(pm => model.Permissions.All(permission => permission != pm.PermissionId))
                .ToList();

            foreach (var permission in toAdd)
            {
                _repository.InsertAsync(new ApplicationRolePermission
                {
                    PermissionId = permission,
                    CreationTime = Clock.Now,
                    CreatorUser = _authService.User,
                    IsActive = true,
                    TenantId = _authService.TenantId,
                    RolId = model.Id
                });
            }

            foreach (var permission in toRemove)
            {
                _repository.DeleteSync(permission);
            }

            await _repository.CommitAsync();

        }

        public async Task ValidateModel(UpdateRoleModel model)
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
                    .AnyAsync<ApplicationRol>(rol =>
                        rol.Id != model.Id
                        && rol.Name.ToUpper() == model.Name.ToUpper());

            if (roleExist)
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Name", ExceptionEnums.ExceptionCode.EmailDuplicate));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("TRANSACTION CANCELLED", "ROLE", this._exceptionFactory.Exceptions);
        }
    }
}