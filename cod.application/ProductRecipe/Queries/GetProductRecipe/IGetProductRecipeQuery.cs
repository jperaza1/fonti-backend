using cod.application.ProductRecipe.Common;
using System.Threading.Tasks;

namespace cod.application.ProductRecipe.Queries.GetProductRecipe
{
    public interface IGetProductRecipeQuery
    {
        Task<ProductRecipeModel> Execute(int productId);
    }
}
