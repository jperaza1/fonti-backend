using cod.application.Category.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.Category.Queries.GetSubCategories
{
    public interface IGetSubCategoriesQuery
    {
        Task<List<SubCategoryModel>> Execute(int categoryId);
    }
}
