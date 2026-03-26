using cod.application.Category.Common;
using System.Threading.Tasks;

namespace cod.application.Category.Queries.GetCategory
{
    public interface IGetCategoryQuery
    {
        Task<CategoryModel> Execute(int id);
    }
}
