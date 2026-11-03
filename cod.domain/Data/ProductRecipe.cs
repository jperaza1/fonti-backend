using cod.domain.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace cod.domain.Data
{
    [Table("ProductRecipes", Schema = "data")]
    public class ProductRecipe : Entity
    {
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public decimal UnitsPerPackage { get; set; }
        public virtual ICollection<ProductRecipeDetail> Details { get; set; }
    }
}
