using cod.domain.Common;
using cod.domain.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace cod.domain.Operation
{
    [Table("ProductionOrders", Schema = "operation")]
    public class ProductionOrder : Entity
    {
        public DateTime ProductionDate { get; set; }
        public int ShiftTypeId { get; set; }
        [ForeignKey("ShiftTypeId")]
        public virtual Data.Type ShiftType { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public decimal Quantity { get; set; }
        public string Note { get; set; }
        public int? ProductionShiftId { get; set; }
        [ForeignKey("ProductionShiftId")]
        public virtual ProductionShift ProductionShift { get; set; }
    }
}
