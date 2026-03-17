using cod.domain.Common;
using cod.domain.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.domain.Operation
{
    [Table("PurchaseOrders", Schema = "operation")]
    public class PurchaseOrder : Entity
    {
        public string Reference { get; set; }
        public DateTime Date { get; set; }
        public int VendorId { get; set; }
        [ForeignKey("VendorId")]
        public virtual Vendor Vendor { get; set; }
        public int CafeteriaId { get; set; }
        [ForeignKey("CafeteriaId")]
        public virtual Cafeteria Cafeteria { get; set; }
        public double  Total { get; set; }
        public PurchaseOrderStatus OrderStatus { get; set; }
        public string ApprovedUser { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string ProcessingUser { get; set; }
        public DateTime? ProcessingAt { get; set; }
        public string CompletedUser { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string ReviewedUser { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string ReasonRejection { get; set; }
        public string RejectedUser { get; set; }
        public DateTime? RejectedAt { get; set; }
        public string ReasonCanceled { get; set; }
        public string TypeInoice { get; set; }
        public string CanceledUser { get; set; }
        public DateTime? CanceledAt { get; set; }
        public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
