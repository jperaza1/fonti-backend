using cod.application.ProductRecipe.Common;
using System.Threading.Tasks;

namespace cod.application.ProductRecipe.Commands.SaveProductRecipe
{
    public interface ISaveProductRecipeCommand
    {
        Task Execute(ProductRecipeModel model);
    }
}
