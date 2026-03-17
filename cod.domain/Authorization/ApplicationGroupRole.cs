using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using cod.domain.Common;

namespace cod.domain.Authorization
{
    [Table("ApplicationGroupRoles", Schema = "auth")]
    public class ApplicationGroupRole : Entity
    {
        public string Name { get; set; }
        public int? GroupHeaderId { get; set; }
        [ForeignKey("GroupHeaderId")]
        public virtual ApplicationGroupRole GroupHeader { get; set; }
        public virtual ICollection<ApplicationGroupRolePermission> GroupRolePermissions { get; set; }
    }
}
