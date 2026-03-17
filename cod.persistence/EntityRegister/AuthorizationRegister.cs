using Microsoft.EntityFrameworkCore;
using cod.domain.Authorization;

namespace cod.persistence.Implementations
{
    public partial class Repository
    {
        public DbSet<ApplicationGroupRole> ApplicationGroupRole { get; set; }
        public DbSet<ApplicationGroupRolePermission> ApplicationGroupRolePermission { get; set; }
        public DbSet<ApplicationRol> ApplicationRol { get; set; }
        public DbSet<ApplicationRolePermission> ApplicationRolePermission { get; set; }
        public DbSet<ApplicationSubscriptionUser> ApplicationSubscriptionUser { get; set; }
        public DbSet<ApplicationTenant> ApplicationTenant { get; set; }
        public DbSet<ApplicationTenantPermission> ApplicationTenantPermission { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<ApplicationUserAlert> ApplicationUserAlerts { get; set; }
        public DbSet<ApplicationUserProfile> ApplicationUserProfile { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRole { get; set; }
    }
}
