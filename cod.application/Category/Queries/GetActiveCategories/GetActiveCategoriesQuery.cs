using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Category.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.Category.Queries.GetActiveCategories
{
    public class GetActiveCategoriesQuery : BaseQuery, IGetActiveCategoriesQuery
    {
        public GetActiveCategoriesQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<CategoryModel>> Execute()
        {
            return await _repository.QueryActive<domain.Data.Category>()
                .Select(f => new CategoryModel
                {
                    Id = f.Id,
                    Name = f.Name,
                    IsActive = f.IsActive,
                })
                .OrderBy(f => f.Name)
                .ToListAsync();
        }
    }
}
