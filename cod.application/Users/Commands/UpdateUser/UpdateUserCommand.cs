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

namespace cod.application.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : BaseCommand, IUpdateUserCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AuthService _authService;

        public UpdateUserCommand(IRepository repository,
            IExceptionFactory exceptionFactory,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _authService = authService;
        }

        public async Task Execute(UpdateUserModel model, int id)
        {

            await ValidateModel(model);

            ApplicationUserProfile userProfile = await _repository.FirstOrDefaultAsync<ApplicationUserProfile>(user => user.Id == id);

            List<ApplicationUserRole> roles = await
                _repository
                    .Query<ApplicationUserRole>(rol =>
                        rol.UserId == userProfile.UserId)
                    .ToListAsync();

            ApplicationUser sosUser = await _userManager.FindByIdAsync(userProfile.UserId);

            await _repository
                .UpdateAsync<ApplicationUserProfile>(profile => profile.Id == id,
                    newValues => new ApplicationUserProfile
                    {
                        UserType = model.UserType,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Routes = UserRoutes.Serialize(model.UserType, model.Routes),
                    });

            if (model.ChangeUsername)
            {
                sosUser.UserName = model.NewUserName;
                sosUser.NormalizedUserName = model.NewUserName.ToUpper();
            }

            await UpdateUserAlerts(model, sosUser.Id);

            ValidateRoles(model, roles, sosUser.Id);

            await _userManager.UpdateAsync(sosUser);

            await _repository.CommitAsync();

            await ValidatePermissions(sosUser, model.RoleIds);

            if (model.ChangePassword)
            {
                string tokenChangePassword = await _userManager.GeneratePasswordResetTokenAsync(sosUser);
                await _userManager.ResetPasswordAsync(sosUser, tokenChangePassword, model.NewPassword);
            }
        }

        private async Task UpdateUserAlerts(UpdateUserModel model, string sosUserId)
        {
            var addedAlerts = await _repository.QueryActive<ApplicationUserAlert>()
                .Where(userAlert => userAlert.UserId == sosUserId)
                .Select(userAlert => userAlert.UserAlertOption)
                .ToListAsync();

            var selectedAlerts = model.UserAlerts.Select(alert => (int)alert).ToList();

            await _repository.UpdateAsync<ApplicationUserAlert>(
                userAlert => userAlert.UserId == sosUserId && selectedAlerts.All(userAlertOption => userAlertOption != (int)userAlert.UserAlertOption),
                userAlert => new ApplicationUserAlert()
                {
                    IsActive = false,
                    DisableDateTime = Clock.Now,
                    DisableUser = _authService.User
                }
            );

            var toAddAlerts = model.UserAlerts.Where(
                alertOption => addedAlerts.All(addedChart => addedChart != alertOption)
            ).ToList();

            foreach (var addAlert in toAddAlerts)
            {
                _repository.InsertAsync(new ApplicationUserAlert()
                {
                    UserId = sosUserId,
                    UserAlertOption = addAlert
                });
            }
        }

        public void ValidateRoles(UpdateUserModel model, List<ApplicationUserRole> roles, string userId)
        {


            var toAdd = model.RoleIds
                .Where(rol => roles.All(db => db.RolId != rol)).ToList();

            var toRemove = roles
                .Where(pm => model.RoleIds.All(rol => rol != pm.RolId)).ToList();

            foreach (var rolId in toAdd)
            {
                _repository.InsertAsync(new ApplicationUserRole
                {
                    RolId = rolId,
                    UserId = userId,
                    CreationTime = Clock.Now,
                    CreatorUser = _authService.User,
                    IsActive = true
                });
            }

            foreach (var rol in toRemove)
            {
                _repository.DeleteSync(rol);
            }
        }

        public async Task ValidatePermissions(ApplicationUser sosUser, List<int> newRoleIds)
        {

            var permissionsForAssign = await
                _repository
                    .Query<ApplicationRolePermission>
                        (rolePermission => newRoleIds.Any(rol => rol == rolePermission.RolId))
                    .Select(rolePermission => rolePermission.Role.Name)
                    .ToListAsync();

            IList<string> permissionAssigned = await _userManager.GetRolesAsync(sosUser);

            List<string> toAdd = permissionsForAssign
                .Where(permission => permissionAssigned.All(db => db != permission))
                .ToList();

            List<string> toRemove = permissionAssigned
                .Where(pm => permissionsForAssign.All(permission => permission != pm))
                .ToList();

            if (toRemove.Any())
                await _userManager.RemoveFromRolesAsync(sosUser, toRemove);

            if (toAdd.Any())
                await _userManager.AddToRolesAsync(sosUser, toAdd);

        }

        public async Task ValidateModel(UpdateUserModel model)
        {
            if (!model.RoleIds.Any())
                this._exceptionFactory.Add("roleIds", this._exceptionFactory.GetMessage("Roles", ExceptionEnums.ExceptionCode.Required));
            if (model.FirstName.NullOrEmpty())
                this._exceptionFactory.Add("firstName", this._exceptionFactory.GetMessage("firstName", ExceptionEnums.ExceptionCode.Required));
            if (model.LastName.NullOrEmpty())
                this._exceptionFactory.Add("lastName", this._exceptionFactory.GetMessage("lastName", ExceptionEnums.ExceptionCode.Required));
            if (model.ChangeUsername && model.NewUserName.NullOrEmpty())
                this._exceptionFactory.Add("newUserName", this._exceptionFactory.GetMessage("New Username", ExceptionEnums.ExceptionCode.Required));
            if (model.ChangePassword && model.NewPassword.NullOrEmpty())
                this._exceptionFactory.Add("newPassword", this._exceptionFactory.GetMessage("New Password", ExceptionEnums.ExceptionCode.Required));

            await UserRoutes.Validate(_repository, _exceptionFactory, model.UserType, model.Routes);

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "USER", this._exceptionFactory.Exceptions);

            if (model.ChangeUsername)
            {
                ApplicationUser sosUser = await _userManager.FindByNameAsync(model.NewUserName);
                if (sosUser != null)
                    this._exceptionFactory.Add("newUserName", this._exceptionFactory.GetMessage("New Username", ExceptionEnums.ExceptionCode.Duplicate));

                if (!this._exceptionFactory.IsValid())
                    throw new ApplicationException("CANCELED", "USER", this._exceptionFactory.Exceptions);
            }
        }
    }
}