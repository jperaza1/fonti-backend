using cod.domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace cod.domain.Data
{
    [Table("ProductVendors", Schema = "data")]
    public class ProductVendor : Entity
    {
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public int VendorId { get; set; }
        [ForeignKey("VendorId")]
        public virtual Vendor Vendor { get; set; }
        public int UnitOfMeasureId { get; set; }
        [ForeignKey("UnitOfMeasureId")]
        public virtual UnitOfMeasure UnitOfMeasure { get; set; }
        public int Unit { get; set; }
        public decimal Cost { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Discount { get; set; }
    }
}
