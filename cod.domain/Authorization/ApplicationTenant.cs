using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using cod.domain.Common;
using cod.domain.Data;

namespace cod.domain.Authorization
{
    [Table("ApplicationTenants", Schema = "auth")]
    public class ApplicationTenant : Entity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public byte[] Logo { get; set; }
        public virtual ICollection<ApplicationTenantPermission> SosTenantPermissions { get; set; }
    }
}
