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
        public enum PurchaseOrderStatus
        {
            [Description("Creado")]
            Created = 1,
            [Description("Aprobado")]
            Approved,
            [Description("Procesando")]
            Processing,
            [Description("Pendiente Revision")]
            Review,
            [Description("Completado")]
            Completed,
            [Description("Rechazado")]
            Rejected,
            [Description("Cancelada")]
            Canceled
        }

        public enum SpecialServiceStatus
        {
            [Description("Creado")]
            Created = 1,
            [Description("Aprobado")]
            Approved,
            [Description("Rechazado")]
            Rejected
        }
    }
    
}
