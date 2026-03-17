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
    [Table("SpecialServiceDetails", Schema = "operation")]
    public class SpecialServiceDetail : Entity
    {
        public int SpecialServiceId { get; set; }
        [ForeignKey("SpecialServiceId")]
        public virtual SpecialService SpecialService { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public double Quantity { get; set; }
    }
}
