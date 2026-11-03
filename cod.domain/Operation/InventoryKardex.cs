using cod.domain.Common;
using cod.domain.Data;
using cod.domain.Features.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace cod.domain.Operation
{
    [Table("InventoryKardex", Schema = "operation")]
    public class InventoryKardex : Entity
    {
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public DateTime MovementDate { get; set; }
        public KardexMovementType MovementType { get; set; }
        public decimal Quantity { get; set; }
        public decimal Balance { get; set; }
        public decimal? UnitCost { get; set; }
        public int? DispatchControlId { get; set; }
        [ForeignKey("DispatchControlId")]
        public virtual DispatchControl DispatchControl { get; set; }
        public int? SourceProductId { get; set; }
        [ForeignKey("SourceProductId")]
        public virtual Product SourceProduct { get; set; }
        public int? ProductionOrderId { get; set; }
        [ForeignKey("ProductionOrderId")]
        public virtual ProductionOrder ProductionOrder { get; set; }
        public string Note { get; set; }
    }
}
