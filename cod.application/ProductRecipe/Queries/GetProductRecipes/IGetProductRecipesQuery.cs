using cod.application.ProductRecipe.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.ProductRecipe.Queries.GetProductRecipes
{
    public interface IGetProductRecipesQuery
    {
        Task<List<ProductRecipeModel>> Execute();
    }
}
