using System.ComponentModel;

namespace cod.domain.Features.Enums
{
    public enum WarehouseTransferStatus
    {
        [Description("Pendiente")]
        Pending = 1,
        [Description("Aceptada")]
        Accepted = 2,
        [Description("Rechazada")]
        Rejected = 3
    }
}
