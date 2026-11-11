using cod.domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace cod.domain.Operation
{
    [Table("DispatchTransfer", Schema = "operation")]
    public class DispatchTransfer : Entity
    {
        public int FromDispatchControlId { get; set; }
        [ForeignKey("FromDispatchControlId")]
        public virtual DispatchControl FromDispatchControl { get; set; }
        public int ToDispatchControlId { get; set; }
        [ForeignKey("ToDispatchControlId")]
        public virtual DispatchControl ToDispatchControl { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
