using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.UnitOfMeasure.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.UnitOfMeasure.Queries.GetUnitOfMeasures
{
    public class GetUnitOfMeasuresQuery : BaseQuery, IGetUnitOfMeasuresQuery
    {
        public GetUnitOfMeasuresQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<UnitOfMeasureModel>> Execute()
        {
            return await _repository.Query<domain.Data.UnitOfMeasure>()
                .Select(f => new UnitOfMeasureModel
                {
                    Id = f.Id,
                    Name = f.Name,
                    IsActive = f.IsActive
                })
                .OrderBy(f => f.Name)
                .ToListAsync();
        }
    }
}
