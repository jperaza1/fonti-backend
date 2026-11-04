using System.ComponentModel;

namespace cod.domain.Features.Enums
{
    public partial class DomainEnums
    {
        public enum PaymentScheduleStatus
        {
            [Description("Pendiente")]
            Pending = 1,
            [Description("Pago Parcial")]
            Partial = 2,
            [Description("Pagado")]
            Paid = 3
        }
    }
}
