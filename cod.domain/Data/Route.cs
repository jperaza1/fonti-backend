using cod.domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Data
{
    [Table("Routes", Schema = "data")]
    public class Route : Entity
    {
        public string Name { get; set; }
        public string Vendor { get; set; }
        public int Order { get; set; }
        public bool DeliversRegalia { get; set; }
        public virtual ICollection<ProductRoutePrice> ProductRoutePrices { get; set; }
        public virtual Truck Truck { get; set; }
    }
}
