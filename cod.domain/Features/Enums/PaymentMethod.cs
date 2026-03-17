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
        public enum PaymentMethod
        {
            [Description("Efectivo")]
            Cash = 1,
            [Description("Transferencia")]
            Transfer,
            [Description("Cheque")]
            Check
        }
    }
}
