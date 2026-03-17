using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cod.domain.Authorization;
using cod.domain.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using cod.application.Features.Exception;
using cod.application.Features.Extensions;
using cod.application.Interfaces;
using cod.domain.Features.Enums;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Tenant.Commands.CreateTenant
{
    public class CreateTenantCommand : ICreateTenantCommand
    {
        private readonly string _passwordDefault = "qwe123";
        private readonly IRepository _repository;
        private readonly IExceptionFactory _exceptionFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthService _authService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CreateTenantCommand(IRepository repository,
            IExceptionFactory exceptionFactory,
            UserManager<ApplicationUser> userManager,
            AuthService authService,
            RoleManager<IdentityRole> roleManager)
        {
            _repository = repository;
            _exceptionFactory = exceptionFactory;
            _userManager = userManager;
            _authService = authService;
            _roleManager = roleManager;
        }
        public async Task Execute(CreateTenantModel createTenantModel)
        {
            await ValidateModel(createTenantModel);
            ApplicationTenant tenant = new ApplicationTenant
            {
                Name = createTenantModel.Name,
                Address = createTenantModel.Address,
                Email = createTenantModel.Email,
                PhoneNumber = createTenantModel.PhoneNumber,
                Logo = createTenantModel.Logo.ConvertToPicture(),
                SosTenantPermissions = createTenantModel.Permissions.Select(permission => new ApplicationTenantPermission
                {
                    PermissionId = permission,
                    CreationTime = Clock.Now,
                    CreatorUser = _authService.User,
                    IsActive = true
                }).ToList()
            };

            _repository.InsertAsync(tenant);
            await _repository.CommitAsync();

            ApplicationUser tenantUser = new ApplicationUser
            {
                Email = createTenantModel.Email,
                TenantId = tenant.Id,
                UserName = createTenantModel.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            IdentityResult identityResult = await _userManager.CreateAsync(tenantUser, _passwordDefault);

            if (identityResult.Succeeded)
            {

                var user = await _userManager.FindByNameAsync(tenantUser.Email);

                List<string> permissions = await
                    _roleManager.Roles
                        .Where(permission => createTenantModel.Permissions.Contains(permission.Id))
                        .Select(permission => permission.NormalizedName)
                        .ToListAsync();

                await _userManager.AddToRolesAsync(user, permissions);

                ApplicationUserProfile applicationUserProfile = new ApplicationUserProfile
                {
                    UserId = user.Id,
                    Email = tenantUser.Email,
                    UserType = DomainEnums.UserType.General,
                    FirstName = createTenantModel.Name,
                    PhoneNumber = createTenantModel.PhoneNumber,
                    TenantId = tenant.Id
                };

                _repository.InsertWithOutDefaultValues(applicationUserProfile);

                var allConfigurations = await
                    _repository.Query<domain.General.Configuration>()
                        .Select(c => new
                        {
                            c.Key,
                            c.Value
                        })
                        .ToListAsync();

                foreach (var configuration in allConfigurations)
                {
                    _repository.InsertWithOutDefaultValues(new domain.General.Configuration
                    {
                        Key = configuration.Key,
                        Value = configuration.Value,
                        TenantId = tenant.Id
                    });
                }

                await _repository.CommitAsync();
            }

        }

        public async Task ValidateModel(CreateTenantModel model)
        {

            if (model.Permissions == null || !model.Permissions.Any())
                this._exceptionFactory.Add("permissions", this._exceptionFactory.GetMessage("Permissions", ExceptionEnums.ExceptionCode.Required));
            if (model.Name.NullOrEmpty())
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("name", ExceptionEnums.ExceptionCode.Required));
            if (model.Email.NullOrEmpty())
                this._exceptionFactory.Add("email", this._exceptionFactory.GetMessage("email", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "COMPANY", this._exceptionFactory.Exceptions);

            ApplicationUser sosUser = await _userManager.FindByEmailAsync(model.Email);
            if (sosUser != null)
                this._exceptionFactory.Add("email", this._exceptionFactory.GetMessage("email", ExceptionEnums.ExceptionCode.EmailDuplicate));

            bool duplicateTenant = await _repository
                 .AnyAsync<ApplicationTenant>(tenant => tenant.Name.ToUpper() == model.Name.ToUpper());

            if (duplicateTenant)
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("name", ExceptionEnums.ExceptionCode.Duplicate));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "COMPANY", this._exceptionFactory.Exceptions);
        }
    }
}