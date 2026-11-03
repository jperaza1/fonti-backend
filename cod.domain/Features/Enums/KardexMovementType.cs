using System.ComponentModel;

namespace cod.domain.Features.Enums
{
    public enum KardexMovementType
    {
        [Description("Entrada")]
        Entry = 1,
        [Description("Salida")]
        Exit = 2,
        [Description("Recarga")]
        Reload = 3,
        [Description("Retorno")]
        Return = 4,
        [Description("Producción")]
        Production = 5,
        [Description("Mala calidad")]
        QualityExit = 6,
        [Description("Pérdida")]
        Loss = 7
    }
}
