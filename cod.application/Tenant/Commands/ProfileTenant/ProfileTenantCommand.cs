using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cod.application.Features.Exception;
using cod.application.Features.Extensions;
using cod.application.Interfaces;
using cod.application.Tenant.Commands.UpdateTenant;
using cod.domain.Authorization;

namespace cod.application.Tenant.Commands.ProfileTenant
{
    public class ProfileTenantCommand : IProfileTenantCommand
    {
        private readonly IRepository _repository;
        private readonly IExceptionFactory _exceptionFactory;

        public ProfileTenantCommand(IRepository repository,
            IExceptionFactory exceptionFactory)
        {
            _repository = repository;
            _exceptionFactory = exceptionFactory;
        }
        public async Task Execute(ProfileTenantModel model, int id)
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

            await _repository.CommitAsync();
        }

        public async Task ValidateModel(ProfileTenantModel model, int id)
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