using cod.domain.Common;
using cod.domain.Operation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Data
{
    [Table("ProductRoutePrices", Schema = "data")]
    public class ProductRoutePrice : Entity
    {
        public int RouteId { get; set; }
        [ForeignKey("RouteId")]
        public virtual Route Route { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public double Price { get; set; }
    }
}
