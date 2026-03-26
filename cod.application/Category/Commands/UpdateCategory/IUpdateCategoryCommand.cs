using cod.application.Category.Common;
using System.Threading.Tasks;

namespace cod.application.Category.Commands.UpdateCategory
{
    public interface IUpdateCategoryCommand
    {
        Task Execute(CategoryModel model, int id);
    }
}
