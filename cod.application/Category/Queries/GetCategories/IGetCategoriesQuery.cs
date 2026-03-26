using cod.application.Category.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.Category.Queries.GetCategories
{
    public interface IGetCategoriesQuery
    {
        Task<List<CategoryModel>> Execute();
    }
}
