using System.ComponentModel;

namespace cod.domain.Features.Enums
{
    public enum PriceApprovalStatus
    {
        [Description("Pendiente")]
        Pending = 1,
        [Description("Aprobado")]
        Approved = 2,
        [Description("Rechazado")]
        Rejected = 3
    }
}
