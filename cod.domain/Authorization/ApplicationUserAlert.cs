using cod.domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.domain.Authorization
{
    [Table("ApplicationUserAlerts", Schema = "auth")]
    public class ApplicationUserAlert : EntityTenantable
    {
        [MaxLength(450)]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public UserAlertOption UserAlertOption { get; set; }
    }
}
