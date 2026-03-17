using Microsoft.AspNetCore.Identity;
using cod.domain.Authorization;
using cod.domain.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace cod.application.Main.Session
{
    public class SessionService : ISessionService
    {
        private readonly AuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(AuthService authService,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetUserId()
        {
            ApplicationUser sosUser = await _userManager.FindByNameAsync(_authService.User);
            return sosUser.Id;
        }

        public async Task<ApplicationUserProfile> GetUserProfile()
        {
            ApplicationUser sosUser = await _userManager.FindByNameAsync(_authService.User);
            return sosUser.ApplicationUserProfile;
        }
    }
}
