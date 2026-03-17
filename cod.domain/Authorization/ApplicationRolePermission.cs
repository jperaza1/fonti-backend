using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using cod.domain.Common;

namespace cod.domain.Authorization
{
    [Table("ApplicationRolePermissions", Schema = "auth")]
    public class ApplicationRolePermission : EntityTenantable
    {
        public int RolId { get; set; }
        [ForeignKey("RolId")]
        public virtual ApplicationRol Rol { get; set; }
        public string PermissionId { get; set; }
        [ForeignKey("PermissionId")]
        public virtual IdentityRole Role { get; set; }
    }
}
