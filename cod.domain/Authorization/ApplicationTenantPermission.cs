using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Identity;
using cod.domain.Common;

namespace cod.domain.Authorization
{
    [Table("ApplicationTenantPermissions", Schema = "auth")]
    public class ApplicationTenantPermission : Entity
    {
        public int TenantId { get; set; }
        [ForeignKey("TenantId")]
        public virtual ApplicationTenant Tenant { get; set; }
        [MaxLength(450)]
        public string PermissionId { get; set; }
        [ForeignKey("PermissionId")]
        public virtual IdentityRole Role { get; set; }
    }
}
