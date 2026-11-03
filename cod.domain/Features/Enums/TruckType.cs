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
        public enum TruckType
        {
            TT = 1,
            TK = 2,
            TR = 3,
            BS = 4
        }

        public enum TruckStatus
        {
            [Description("Disponible")]
            Available = 1,
            [Description("Mantenimiento")]
            Maintenance = 2
        }
    }
    
}
