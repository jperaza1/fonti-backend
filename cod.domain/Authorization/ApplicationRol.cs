using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using cod.domain.Common;

namespace cod.domain.Authorization
{
    [Table("ApplicationRoles", Schema = "auth")]
    public class ApplicationRol : EntityTenantable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<ApplicationRolePermission> ApplicationRolePermissions { get; set; }
    }
}
