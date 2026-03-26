using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Category.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.Category.Queries.GetSubCategories
{
    public class GetSubCategoriesQuery : BaseQuery, IGetSubCategoriesQuery
    {
        public GetSubCategoriesQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<SubCategoryModel>> Execute(int categoryId)
        {
            return await _repository.QueryActive<domain.Data.SubCategory>()
                .Where(f => f.CategoryId == categoryId)
                .Select(g => new SubCategoryModel
                {
                    Id = g.Id,
                    CategoryId = g.CategoryId,
                    Name = g.Name,
                }).ToListAsync();
        }
    }
}
