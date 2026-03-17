using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using cod.domain.Common;

namespace cod.domain.Authorization
{
    [Table("ApplicationUserRoles", Schema = "auth")]
    public class ApplicationUserRole : Entity
    {
        [MaxLength(450)]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser SosUser { get; set; }
        public int RolId { get; set; }
        [ForeignKey("RolId")]
        public virtual ApplicationRol Rol { get; set; }
    }
}
