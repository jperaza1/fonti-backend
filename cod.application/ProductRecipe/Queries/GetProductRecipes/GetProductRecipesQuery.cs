using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.ProductRecipe.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.ProductRecipe.Queries.GetProductRecipes
{
    public class GetProductRecipesQuery : BaseQuery, IGetProductRecipesQuery
    {
        public GetProductRecipesQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<ProductRecipeModel>> Execute()
        {
            var recipes = await _repository.QueryActive<domain.Data.ProductRecipe>()
                .Select(f => new ProductRecipeModel
                {
                    Id = f.Id,
                    ProductId = f.ProductId,
                    Product = f.Product.Name,
                    UnitsPerPackage = f.UnitsPerPackage,
                    Details = f.Details.Where(d => d.IsActive).Select(d => new ProductRecipeDetailModel
                    {
                        Id = d.Id,
                        RawMaterialProductId = d.RawMaterialProductId,
                        RawMaterial = d.RawMaterialProduct.Name,
                        UnitOfMeasure = d.RawMaterialProduct.UnitOfMeasure.Name,
                        Quantity = d.Quantity,
                        ProducedUnits = d.ProducedUnits,
                        PerPackage = d.PerPackage
                    }).ToList()
                })
                .ToListAsync();

            foreach (var recipe in recipes)
            {
                foreach (var detail in recipe.Details)
                    detail.QuantityPerUnit = InventoryQuantity.PerFinishedUnit(recipe.UnitsPerPackage, detail);
            }

            return recipes;
        }
    }
}
