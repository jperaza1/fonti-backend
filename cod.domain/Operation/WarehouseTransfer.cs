using cod.domain.Common;
using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace cod.domain.Operation
{
    [Table("WarehouseTransfers", Schema = "operation")]
    public class WarehouseTransfer : Entity
    {
        public DateTime Date { get; set; }
        public int FromWarehouseId { get; set; }
        [ForeignKey("FromWarehouseId")]
        public virtual Warehouse FromWarehouse { get; set; }
        public int ToWarehouseId { get; set; }
        [ForeignKey("ToWarehouseId")]
        public virtual Warehouse ToWarehouse { get; set; }
        public string Note { get; set; }
        public WarehouseTransferStatus Status { get; set; }
        public string SentUser { get; set; }
        public string AcceptedUser { get; set; }
        public DateTime? AcceptedAt { get; set; }
        public string RejectedUser { get; set; }
        public DateTime? RejectedAt { get; set; }
        public string Reason { get; set; }
        public virtual ICollection<WarehouseTransferDetail> Details { get; set; }
    }
}
