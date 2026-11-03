using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Features.Enums
{
    public enum PaymentStatus
    {
        [Description("Creado")]
        Created = 1,
        [Description("Procesando")]
        Processing = 2,
        [Description("Cerrado")]
        Closed = 3
    }
}
