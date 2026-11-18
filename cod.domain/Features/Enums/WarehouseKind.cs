using System.ComponentModel;

namespace cod.domain.Features.Enums
{
    public enum WarehouseKind
    {
        [Description("Principal")]
        Principal = 1,
        [Description("Producción")]
        Production = 2,
        [Description("Terminado")]
        Finished = 3
    }
}
