using cod.domain.Common;
using cod.domain.Operation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Rrhh
{
    [Table("WorkExperience", Schema = "rrhh")]
    public class WorkExperience : Entity
    {
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
        public string Company { get; set; }
        public string JobTitle { get; set; }
        public string Duration { get; set; }
        public string ReasonForLeaving { get; set; }
    }
}
