using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Category.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Category.Queries.GetCategory
{
    public class GetCategoryQuery : BaseQuery, IGetCategoryQuery
    {
        public GetCategoryQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<CategoryModel> Execute(int id)
        {
            var category = await _repository.Query<domain.Data.Category>()
                .Where(f => f.Id == id)
                .Select(f => new CategoryModel
                {
                    Id = f.Id,
                    Name = f.Name,
                    IsActive = f.IsActive,
                    SubCategories = f.SubCategories.Select(g => new SubCategoryModel
                    {
                        Id = g.Id,
                        CategoryId = g.CategoryId,
                        Name = g.Name
                    }).ToList()
                }).FirstOrDefaultAsync();

            if (category == null)
                throw new ApplicationException("Categoría no existe", "CATEGORIA");

            return category;
        }
    }
}
