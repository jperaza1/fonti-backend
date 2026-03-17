using System;
using System.Collections.Generic;
using System.Text;

namespace cod.domain.Common
{
    public interface IEntityTenantable
    {
        public int? TenantId { get; set; }
    }
}
