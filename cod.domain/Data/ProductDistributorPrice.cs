using cod.domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace cod.domain.Data
{
    [Table("ProductDistributorPrices", Schema = "data")]
    public class ProductDistributorPrice : Entity
    {
        public int DistributorId { get; set; }
        [ForeignKey("DistributorId")]
        public virtual Distributor Distributor { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public double Price { get; set; }
    }
}
