using cod.domain.Common;
using cod.domain.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace cod.domain.Operation
{
    [Table("ProductStock", Schema = "operation")]
    public class ProductStock : Entity
    {
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public decimal Quantity { get; set; }
    }
}
