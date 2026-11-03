using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.UnitOfMeasure.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.UnitOfMeasure.Queries.GetActiveUnitOfMeasures
{
    public class GetActiveUnitOfMeasuresQuery : BaseQuery, IGetActiveUnitOfMeasuresQuery
    {
        public GetActiveUnitOfMeasuresQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<UnitOfMeasureModel>> Execute()
        {
            return await _repository.QueryActive<domain.Data.UnitOfMeasure>()
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
