using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using cod.domain.Common;

namespace cod.domain.Authorization
{
    [Table("ApplicationGroupRolePermissions", Schema = "auth")]
    public class ApplicationGroupRolePermission : Entity
    {
        [MaxLength(450)]
        public string PermissionId { get; set; }
        [ForeignKey("PermissionId")]
        public virtual IdentityRole Role { get; set; }
        public int GroupRoleId { get; set; }
        [ForeignKey("GroupRoleId")]
        public virtual ApplicationGroupRole GroupRole { get; set; }
    }
}
