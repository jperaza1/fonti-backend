using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.ProductRecipe.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.ProductRecipe.Queries.GetProductRecipe
{
    public class GetProductRecipeQuery : BaseQuery, IGetProductRecipeQuery
    {
        public GetProductRecipeQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<ProductRecipeModel> Execute(int productId)
        {
            var recipe = await _repository.QueryActive<domain.Data.ProductRecipe>()
                .Where(f => f.ProductId == productId)
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
                .FirstOrDefaultAsync();

            if (recipe == null)
                throw new ApplicationException("El producto no tiene receta", "RECETA");

            foreach (var detail in recipe.Details)
                detail.QuantityPerUnit = InventoryQuantity.PerFinishedUnit(recipe.UnitsPerPackage, detail);

            return recipe;
        }
    }
}
