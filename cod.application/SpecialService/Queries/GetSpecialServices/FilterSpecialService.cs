using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.SpecialService.Queries.GetSpecialServices
{
    public class FilterSpecialService
    {
        public int? CafeteriaId { get; set; }
        public SpecialServiceStatus Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
