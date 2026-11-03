using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace cod.domain.Features
{
    public class AuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public int? TenantId => _httpContextAccessor?.HttpContext?.Request?.GetTenant();
        public int? CarrierId => _httpContextAccessor?.HttpContext?.Request?.GetCarrier();
        public int? CompanyId => _httpContextAccessor.HttpContext?.Request?.GetCompany();
        public string User => _httpContextAccessor?.HttpContext?.User.Identity.Name;
        public string ClientTimezone => _httpContextAccessor.HttpContext?.Request?.GetTimeZone();
        public List<int> Routes => _httpContextAccessor.HttpContext?.Request?.GetRoutes();


        public AuthService(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

    }
}
