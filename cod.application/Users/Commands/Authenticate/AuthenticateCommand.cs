using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using cod.application.Tenant.Queries.GetTenantById;
using cod.domain.Authorization;
using cod.domain.Features;
using cod.domain.Features.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using cod.application.Features.Extensions;
using cod.application.Interfaces;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace cod.application.Users.Commands.Authenticate
{
    public class AuthenticateCommand : IAuthenticateCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IGetTenantByIdQuery _getTenantByIdQuery;
        private readonly IRepository _repository;

        public AuthenticateCommand(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IGetTenantByIdQuery getTenantByIdQuery,
            IRepository repository
            )
        {
            this._userManager = userManager;
            this._configuration = configuration;
            _getTenantByIdQuery = getTenantByIdQuery;
            _repository = repository;
        }
        public async Task<AuthenticateResponseModel> Execute(AuthenticateRequestModel authenticateRequestModel)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(authenticateRequestModel.Username);
            if (user == null)
                return new AuthenticateResponseModel { ErrorMessage = "USER NOT FOUND", Success = false };
            if (!await _userManager.CheckPasswordAsync(user, authenticateRequestModel.Password))
                return new AuthenticateResponseModel { ErrorMessage = "INVALID PASSWORD", Success = false };

            _repository.DisableTenantFilter = true;
            ApplicationUserProfile userProfile = await
                _repository.FirstOrDefaultAsync<ApplicationUserProfile>(profile => profile.UserId == user.Id);
            if (userProfile != null && !userProfile.IsActive)
                return new AuthenticateResponseModel { ErrorMessage = "USER INACTIVE", Success = false };

            int userTenantId = Convert.ToInt32(user.TenantId);
            GetTenantByIdModel userTenant = await _getTenantByIdQuery.Execute(userTenantId);

            if (!userTenant.IsActive)
            {
                return new AuthenticateResponseModel
                {
                    Success = false,
                    ErrorMessage = $"Company inactive, please contact support."
                };
            }

            if (userTenant.Demo && Clock.Now > userTenant.DemoTerminationDate)
            {
                return new AuthenticateResponseModel
                {
                    Success = false,
                    ErrorMessage = $"Demo has finished.Termination Date: {userTenant.DemoTerminationDate.Value:MM/dd/yyyy}"
                };
            }

            IList<string> userRoles = await _userManager.GetRolesAsync(user);

            List<Claim> authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };

            foreach (string rol in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, rol));
            }

            SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: authClaims,
                expires: DateTime.Now.AddYears(1),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return new AuthenticateResponseModel
            {
                Success = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                UserAuth = new UserAuthenticateResponseModel
                {
                    Permissions = userRoles,
                    FullName = userProfile == null ? "ADMINISTRATOR" : $"{userProfile?.FirstName} {userProfile?.LastName}",
                    TenantName = userTenant.Name,
                    TenantId = userTenant.Id,
                    Expiration = userTenant.DemoTerminationDate,
                    UserType = userProfile.IsNull() ? DomainEnums.UserType.General : userProfile.UserType,
                    Routes = userProfile.IsNull() ? new List<int>() : UserRoutes.Parse(userProfile.Routes),
                },
            };

        }
    }
}
