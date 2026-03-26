using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Correlative.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.Correlative.Queries.GetCorrelatives
{
    public class GetCorrelativesQuery : BaseQuery, IGetCorrelativesQuery
    {
        public GetCorrelativesQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<CorrelativeModel>> Execute()
        {
            return await _repository.Query<domain.Data.Correlative>().Select(f => new CorrelativeModel
            {
                Id = f.Id,
                Cai = f.Cai,
                Start = f.Start,
                End = f.End,
                StartDate = f.StartDate,
                EndDate = f.EndDate,
                Current = f.Current,
                EmissionPoint = f.EmissionPoint,
                Establishment = f.Establishment,
                DocumentType = f.DocumentType,
                IsActive = f.IsActive
            }).ToListAsync();
        }
    }
}
