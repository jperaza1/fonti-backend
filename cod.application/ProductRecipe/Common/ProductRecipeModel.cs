using System.Collections.Generic;

namespace cod.application.ProductRecipe.Common
{
    public class ProductRecipeModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Product { get; set; }
        public decimal UnitsPerPackage { get; set; }
        public List<ProductRecipeDetailModel> Details { get; set; }
    }

    public class ProductRecipeDetailModel
    {
        public int Id { get; set; }
        public int RawMaterialProductId { get; set; }
        public string RawMaterial { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal Quantity { get; set; }
        public decimal ProducedUnits { get; set; }
        public bool PerPackage { get; set; }
        public decimal QuantityPerUnit { get; set; }
    }
}
