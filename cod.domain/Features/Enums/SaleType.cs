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
        public enum SaleType
        {
            [Description("Contado")]
            Cash = 1,
            [Description("Credito")]
            Credit,
        }

        public enum SaleTypeDish
        {
            [Description("Ambos")]
            Both = 1,
            [Description("Solo Carnet")]
            CarnetOnly,
            [Description("Sin Carnet")]
            WithOutCarnet
        }
    }
}
