using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Features.Enums
{
    public partial class DomainEnums
    {
        public enum VacationRequestType
        {
            [Description("Solicitadas")]
            Requested = 1,
            [Description("Feriado")]
            Holiday
        }
    }
}
