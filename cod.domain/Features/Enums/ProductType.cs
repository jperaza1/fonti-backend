using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Features.Enums
{
    public enum ProductType
    {
        [Description("Venta")]
        Sale = 1,
        [Description("Compra")]
        Purchase
    }
}
