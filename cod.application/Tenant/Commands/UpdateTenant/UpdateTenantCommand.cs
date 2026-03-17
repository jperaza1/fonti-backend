using System.Linq;
using System.Threading.Tasks;
using cod.domain.Authorization;
using Microsoft.EntityFrameworkCore;
using cod.application.Features.Exception;
using cod.application.Features.Extensions;
using cod.application.Interfaces;

namespace cod.application.Tenant.Commands.UpdateTenant
{
    public class UpdateTenantCommand : IUpdateTenantCommand
    {
        private readonly IRepository _repository;
        private readonly IExceptionFactory _exceptionFactory;

        public UpdateTenantCommand(IRepository repository,
            IExceptionFactory exceptionFactory)
        {
            _repository = repository;
            _exceptionFactory = exceptionFactory;
        }
        public async Task Execute(UpdateTenantModel model, int id)
        {
            await ValidateModel(model, id);

            byte[] logoOfTenant = model.Logo.ConvertToPicture();

            await _repository.UpdateAsync<ApplicationTenant>(tenant => tenant.Id == id,
                newValuesTenants => new ApplicationTenant
                {
                    Name = model.Name,
                    Address = model.Address,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Logo = logoOfTenant
                });

            var permissions = await _repository
                .Query<ApplicationTenantPermission>()
                .Where(tenant => tenant.TenantId == id).ToListAsync();

            var toAdd = model.Permissions
                .Where(permission => permissions.All(db => db.PermissionId != permission))
                .ToList();

            var toRemove = permissions
                .Where(pm => model.Permissions.All(permission => permission != pm.PermissionId))
                .ToList();

            foreach (var permission in toAdd)
            {
                _repository.InsertAsync(new ApplicationTenantPermission
                {
                    PermissionId = permission,
                    TenantId = id
                });
            }

            foreach (var permission in toRemove)
            {
                _repository.DeleteSync(permission);
            }

            await _repository.CommitAsync();
        }

        public async Task ValidateModel(UpdateTenantModel model, int id)
        {
            if (model.Name.NullOrEmpty())
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("name", ExceptionEnums.ExceptionCode.Required));
            if (model.Email.NullOrEmpty())
                this._exceptionFactory.Add("email", this._exceptionFactory.GetMessage("email", ExceptionEnums.ExceptionCode.Required));

            bool duplicateTenant = await _repository
                .AnyAsync<ApplicationTenant>(tenant => tenant.Id != id && tenant.Name.ToUpper() == model.Name.ToUpper());

            if (duplicateTenant)
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("name", ExceptionEnums.ExceptionCode.Duplicate));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "COMPANY", this._exceptionFactory.Exceptions);
        }

    }
}