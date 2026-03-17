using cod.domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Data
{
    [Table("VacationPolicies", Schema = "data")]
    public class VacationPolicy : Entity
    {
        public int FromYear { get; set; }
        public int ToYear { get; set; }
        public int DayPerYear { get; set; }
    }
}
