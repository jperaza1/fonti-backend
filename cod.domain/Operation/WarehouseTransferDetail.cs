using cod.domain.Common;
using cod.domain.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace cod.domain.Operation
{
    [Table("WarehouseTransferDetails", Schema = "operation")]
    public class WarehouseTransferDetail : Entity
    {
        public int WarehouseTransferId { get; set; }
        [ForeignKey("WarehouseTransferId")]
        public virtual WarehouseTransfer WarehouseTransfer { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public decimal Quantity { get; set; }
    }
}
