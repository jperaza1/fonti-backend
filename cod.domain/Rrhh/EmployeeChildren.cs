using cod.domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Rrhh
{
    [Table("EmployeeChildren", Schema = "rrhh")]
    public class EmployeeChildren : Entity
    {
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
