using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading;
using cod.domain.Common;
using cod.domain.Data;
using cod.domain.Features.Enums;

namespace cod.domain.Authorization
{
    [Table("ApplicationUserProfiles", Schema = "auth")]
    public class ApplicationUserProfile : EntityTenantable
    {
        [MaxLength(450)]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Routes { get; set; }

        [DefaultValue(1)]
        public DomainEnums.UserType UserType { get; set; }
    }
}
