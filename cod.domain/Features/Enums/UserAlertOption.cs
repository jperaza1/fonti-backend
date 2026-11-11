using System.ComponentModel;

namespace cod.domain.Features.Enums
{
    public partial class DomainEnums
    {
        public enum UserAlertOption
        {
            [Description("Facturas Pendientes")]
            PendingInvoice = 1,
            [Description("Correlativos por vencer")]
            CorrelativeExpiring = 2,
        }
    }
}
