using cod.domain.Common;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.domain.Rrhh
{
    [Table("VacationRequested", Schema = "rrhh")]
    public class VacationRequested : Entity
    {
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int TotalDays { get; set; }
        public VacationRequestType RequestType { get; set; }
    }
}
