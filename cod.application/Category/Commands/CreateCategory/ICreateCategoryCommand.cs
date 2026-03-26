using cod.application.Category.Common;
using System.Threading.Tasks;

namespace cod.application.Category.Commands.CreateCategory
{
    public interface ICreateCategoryCommand
    {
        Task Execute(CategoryModel model);
    }
}
