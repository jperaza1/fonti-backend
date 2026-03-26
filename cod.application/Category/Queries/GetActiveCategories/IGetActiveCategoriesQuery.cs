using cod.application.Category.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.Category.Queries.GetActiveCategories
{
    public interface IGetActiveCategoriesQuery
    {
        Task<List<CategoryModel>> Execute();
    }
}
