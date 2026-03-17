using cod.domain.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace cod.domain.Common
{
    public class EntityTenantable : Entity, IEntityTenantable
    {
        public int? TenantId { get; set; }
        [ForeignKey("TenantId")]
        public virtual ApplicationTenant Tenant { get; set; }
    }
}
