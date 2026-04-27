using cod.domain.Common;
using cod.domain.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Operation
{
    [Table("InventoryDetails", Schema = "operation")]
    public class InventoryDetail : Entity
    {
        public int InventoryId { get; set; }
        [ForeignKey("InventoryId")]
        public virtual Inventory Inventory { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public double Cost { get; set; }
        public double UnitCost { get; set; }
        public double UnitQuantity { get; set; }
        public double QuantityPack { get; set; }
        public double SubTotal { get; set; }
        public double Tax { get; set; }
        public double TaxPercentage { get; set; }
        public double Discount { get; set; }
        public double DiscountPercentage { get; set; }
        public double Total { get; set; }

    }
}
