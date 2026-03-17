using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Features.Enums
{
    public partial class DomainEnums
    {
        public enum ApplicationStatus
        {
            Active = 1,
            Hired = 2,
            NeverFinishedProcess = 3,
            Suspect = 4,
            Other = 5
        }
    }
}
