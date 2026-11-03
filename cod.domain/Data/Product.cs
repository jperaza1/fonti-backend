using cod.domain.Common;
using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Data
{
    [Table("Products", Schema = "data")]
    public class Product : Entity
    {
        public string Name { get; set; }
        public ProductType ProductType { get; set; }
        public bool Tax { get; set; }
        public decimal? Cost { get; set; }
        public int? UnitOfMeasureId { get; set; }
        [ForeignKey("UnitOfMeasureId")]
        public virtual UnitOfMeasure UnitOfMeasure { get; set; }
        public virtual ICollection<ProductVendor> ProductVendors { get; set; }
    }
}
