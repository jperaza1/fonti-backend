using cod.domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Rrhh
{
    [Table("VacationPeriod", Schema = "rrhh")]
    public class VacationPeriod : Entity
    {
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }

        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public int ServiceYear { get; set; }
        public int EntitledDays { get; set; }
    }
}
