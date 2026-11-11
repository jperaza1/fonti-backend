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

        public enum DispatchControlType
        {
            [Description("Contado")]
            Exit = 1,
            [Description("Contado")]
            Reload,
            [Description("Contado")]
            Return,
            [Description("Traspaso salida")]
            TransferOut = 4,
            [Description("Traspaso entrada")]
            TransferIn = 5
        }

        public enum DispatchControlStatus
        {
            [Description("Abierto")]
            Open = 1,
            [Description("Terminado")]
            Close
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
