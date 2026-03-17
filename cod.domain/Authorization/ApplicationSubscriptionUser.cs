using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using cod.domain.Common;

namespace cod.domain.Authorization
{
    [Table("ApplicationSubscriptionUser", Schema = "auth")]
    public class ApplicationSubscriptionUser : EntityTenantable
    {
        [MaxLength(450)]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser SosUser { get; set; }
        public string JsonSubscription { get; set; }
    }
}
