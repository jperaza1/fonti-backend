using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cod.domain.Authorization;
using cod.domain.Features;
using cod.domain.Features.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Features.Extensions;
using cod.application.Interfaces;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Users.Commands.CreateUser
{
    public class CreateUserCommand : BaseCommand, ICreateUserCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthService _authService;

        public CreateUserCommand(IRepository repository,
            IExceptionFactory exceptionFactory,
            UserManager<ApplicationUser> userManager,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _userManager = userManager;
            _authService = authService;
        }
        public async Task Execute(CreateUserModel model)
        {
            await ValidateModel(model);

            ApplicationUser user = new ApplicationUser
            {
                Email = model.Email,
                TenantId = _authService.TenantId,
                UserName = model.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            IdentityResult identityResult = await _userManager.CreateAsync(user, model.Password);

            if (identityResult.Succeeded)
            {
                user = await _userManager.FindByNameAsync(model.Username);

                List<string> permissions = await
                    _repository
                        .Query<ApplicationRolePermission>(permission => model.RoleIds.Contains(permission.RolId))
                        .Select(permission => permission.Role.Name)
                        .ToListAsync();

                CreateRoles(model.RoleIds, user.Id);
                CreateUserAlerts(model.UserAlerts, user.Id);

                await _userManager.AddToRolesAsync(user, permissions);

                ApplicationUserProfile userProfile = new ApplicationUserProfile
                {
                    UserId = user.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserType = model.UserType,
                };

                _repository.InsertAsync(userProfile);

                await _repository.CommitAsync();
            }
            else
                throw new ApplicationException(identityResult.Errors
                    .Select(error => error.Description)
                    .Aggregate((a, b) => $"{a}, {b}"), "USER");
        }

        private void CreateUserAlerts(List<DomainEnums.UserAlertOption> modelUserAlerts, string userId)
        {
            foreach (var alertOption in modelUserAlerts)
            {
                _repository.InsertAsync(new ApplicationUserAlert
                {
                    UserId = userId,
                    UserAlertOption = alertOption
                });
            }
        }

        private void CreateRoles(List<int> modelRoleIds, string userId)
        {
            foreach (var roleId in modelRoleIds)
            {
                _repository.InsertAsync(new ApplicationUserRole
                {
                    UserId = userId,
                    RolId = roleId,
                    IsActive = true,
                    CreationTime = Clock.Now,
                    CreatorUser = _authService.User
                });
            }
        }

        public async Task ValidateModel(CreateUserModel model)
        {
            if (!model.RoleIds.Any())
                this._exceptionFactory.Add("roleIds", this._exceptionFactory.GetMessage("Roles", ExceptionEnums.ExceptionCode.Required));
            if (model.Username.NullOrEmpty())
                this._exceptionFactory.Add("username", this._exceptionFactory.GetMessage("username", ExceptionEnums.ExceptionCode.Required));
            if (model.Password.NullOrEmpty())
                this._exceptionFactory.Add("password", this._exceptionFactory.GetMessage("password", ExceptionEnums.ExceptionCode.Required));
            if (model.FirstName.NullOrEmpty())
                this._exceptionFactory.Add("firstName", this._exceptionFactory.GetMessage("firstName", ExceptionEnums.ExceptionCode.Required));
            if (model.LastName.NullOrEmpty())
                this._exceptionFactory.Add("lastName", this._exceptionFactory.GetMessage("lastName", ExceptionEnums.ExceptionCode.Required));
            if (model.Email.NullOrEmpty())
                this._exceptionFactory.Add("email", this._exceptionFactory.GetMessage("email", ExceptionEnums.ExceptionCode.Required));

            if(model.UserType == DomainEnums.UserType.Supervisor && (!model.CafeteriaId.HasValue || model.CafeteriaId <= 0))
                this._exceptionFactory.Add("cafeteria", this._exceptionFactory.GetMessage("Cafeteria", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "USER", this._exceptionFactory.Exceptions);

            ApplicationUser sosUser = await _userManager.FindByEmailAsync(model.Email);
            if (sosUser != null)
                this._exceptionFactory.Add("email", this._exceptionFactory.GetMessage("email", ExceptionEnums.ExceptionCode.EmailDuplicate));

            sosUser = await _userManager.FindByNameAsync(model.Username);
            if (sosUser != null)
                this._exceptionFactory.Add("username", this._exceptionFactory.GetMessage("username", ExceptionEnums.ExceptionCode.Duplicate));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "USER", this._exceptionFactory.Exceptions);

        }
    }
}