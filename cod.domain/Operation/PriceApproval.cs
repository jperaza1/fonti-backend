using cod.domain.Common;
using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace cod.domain.Operation
{
    [Table("PriceApproval", Schema = "operation")]
    public class PriceApproval : Entity
    {
        public string Token { get; set; }
        public DateTime DatePriceApproval { get; set; }
        public int PurchaseOrderId { get; set; }
        [ForeignKey("PurchaseOrderId")]
        public virtual PurchaseOrder PurchaseOrder { get; set; }
        public PriceApprovalStatus Status { get; set; }
        public string Notes { get; set; }
        public string RejectedUser { get; set; }
        public DateTime? RejectedAt { get; set; }
        public string ApprovedUser { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public virtual ICollection<PriceApprovalDetail> PriceApprovalDetails { get; set; }
    }
}
