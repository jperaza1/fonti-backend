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
        public enum DetailStatus
        {
            [Description("Pendiente")]
            Pending = 1,
            [Description("Recibido")]
            Received,
            [Description("Recibido Parcialmente")]
            Partially_Received,
            [Description("Diferencia en cantidad")]
            Quantity_Mismatch,
            [Description("Sin Existencias")]
            Out_Of_Stock,
            [Description("Precio Diferente")]
            Price_Changed,
            [Description("Precio y Cantidad Diferente")]
            Prince_Quantity_Mismatch
        }
    }
}
