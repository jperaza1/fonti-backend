using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Features.Enums
{
    public enum DishType
    {
        [Description("Basico")]
        Basic = 1,
        [Description("Especial")]
        Special = 2
    }
}
