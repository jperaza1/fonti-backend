using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Correlative.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Correlative.Queries.GetCorrelative
{
    public class GetCorrelativeQuery : BaseQuery, IGetCorrelativeQuery
    {
        public GetCorrelativeQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<CorrelativeModel> Execute(int id)
        {
            var correlative = await _repository.Query<domain.Data.Correlative>()
                .Where(f => f.Id == id)
                .Select(f => new
                {
                    f.Id,
                    f.Cai,
                    f.Start,
                    f.End,
                    f.StartDate,
                    f.EndDate,
                    f.Current,
                    f.EmissionPoint,
                    f.Establishment,
                    f.DocumentType,
                    f.IsActive
                })
                .FirstOrDefaultAsync();

            if (correlative == null)
                throw new ApplicationException("Correlativo no existe", "CORRELATIVO");

            return new CorrelativeModel
            {
                Id = correlative.Id,
                Cai = correlative.Cai,
                Start = correlative.Start,
                End = correlative.End,
                StartDate = correlative.StartDate,
                EndDate = correlative.EndDate,
                Current = correlative.Current,
                EmissionPoint = correlative.EmissionPoint,
                Establishment = correlative.Establishment,
                DocumentType = correlative.DocumentType,
                IsActive = correlative.IsActive
            };
        }
    }
}
