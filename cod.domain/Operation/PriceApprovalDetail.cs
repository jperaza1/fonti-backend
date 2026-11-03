using cod.domain.Common;
using cod.domain.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace cod.domain.Operation
{
    [Table("PriceApprovalDetails", Schema = "operation")]
    public class PriceApprovalDetail : Entity
    {
        public int PriceApprovalId { get; set; }
        [ForeignKey("PriceApprovalId")]
        public virtual PriceApproval PriceApproval { get; set; }
        public int ProductVendorId { get; set; }
        [ForeignKey("ProductVendorId")]
        public virtual ProductVendor ProductVendor { get; set; }
        public decimal OldCost { get; set; }
        public decimal NewCost { get; set; }
    }
}
