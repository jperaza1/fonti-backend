using cod.domain.Common;
using cod.domain.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace cod.domain.Operation
{
    [Table("ProductionShifts", Schema = "operation")]
    public class ProductionShift : Entity
    {
        public DateTime ProductionDate { get; set; }
        public bool IsClosed { get; set; }
        public int ShiftTypeId { get; set; }
        [ForeignKey("ShiftTypeId")]
        public virtual Data.Type ShiftType { get; set; }
        public virtual ICollection<ProductionOrder> Lines { get; set; }
    }
}
