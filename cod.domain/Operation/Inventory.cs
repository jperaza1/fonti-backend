using cod.domain.Common;
using cod.domain.Data;
using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Operation
{
    [Table("Inventories", Schema = "operation")]
    public class Inventory : Entity
    {
        public string Reference { get; set; }
        public DateTime Date { get; set; }
        public int CafeteriaId { get; set; }
        [ForeignKey("CafeteriaId")]
        public virtual Cafeteria Cafeteria { get; set; }
        public int? RegisterId { get; set; }
        [ForeignKey("RegisterId")]
        public virtual Register Register { get; set; }
        public InventoryType InventoryType { get; set; }
        public bool UseInInventory { get; set; }
        public double Total { get; set; }
        public string ReasonCanceled { get; set; }
        public string CanceledUser { get; set; }
        public DateTime? CanceledAt { get; set; }
        public virtual ICollection<InventoryDetail> InventoryDetails { get; set; }
    }
}
