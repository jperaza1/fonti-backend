using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Features.Enums
{
    public enum InventoryType
    {
        [Description("Comercialización")]
        Commercialization = 2,
        [Description("Producción")]
        Production = 1
    }
}
