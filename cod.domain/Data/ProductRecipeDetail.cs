using cod.domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace cod.domain.Data
{
    [Table("ProductRecipeDetails", Schema = "data")]
    public class ProductRecipeDetail : Entity
    {
        public int ProductRecipeId { get; set; }
        [ForeignKey("ProductRecipeId")]
        public virtual ProductRecipe ProductRecipe { get; set; }
        public int RawMaterialProductId { get; set; }
        [ForeignKey("RawMaterialProductId")]
        public virtual Product RawMaterialProduct { get; set; }
        public decimal Quantity { get; set; }
        public decimal ProducedUnits { get; set; }
        public bool PerPackage { get; set; }
    }
}
