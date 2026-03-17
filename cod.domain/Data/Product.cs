using cod.domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Data
{
    [Table("Products", Schema = "data")]
    public class Product : Entity
    {
        public int VendorId { get; set; }
        [ForeignKey("VendorId")]
        public virtual Vendor Vendor { get; set; }
        public string Name { get; set; }
        public int MeasureId { get; set; }
        [ForeignKey("MeasureId")]
        public virtual Type Measure { get; set; }
        public int Unit { get; set; }
        public double Cost { get; set; }
        public double UnitCost { get; set; }
        public double Tax { get; set; }
        public double Discount { get; set; }
        public bool SpecialServiceUse { get; set; }
    }
}
