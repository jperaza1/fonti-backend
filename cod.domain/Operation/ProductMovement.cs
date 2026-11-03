using cod.domain.Common;
using cod.domain.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Operation
{
    [Table("ProductMovement", Schema = "operation")]
    public class ProductMovement : Entity
    {
        public int DispatchControlDetailId { get; set; }
        [ForeignKey("DispatchControlDetailId")]
        public virtual DispatchControlDetail DispatchControlDetail { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
