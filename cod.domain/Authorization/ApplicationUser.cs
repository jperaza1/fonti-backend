using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using cod.domain.Common;

namespace cod.domain.Authorization
{
    public class ApplicationUser : IdentityUser, IEntityTenantable
    {
        public int? TenantId { get; set; }
        public virtual ICollection<ApplicationSubscriptionUser> ApplicationSubscriptions { get; set; }
        public virtual ApplicationUserProfile ApplicationUserProfile { get; set; }
    }
}
