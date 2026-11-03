using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.UnitOfMeasure.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.UnitOfMeasure.Queries.GetUnitOfMeasure
{
    public class GetUnitOfMeasureQuery : BaseQuery, IGetUnitOfMeasureQuery
    {
        public GetUnitOfMeasureQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<UnitOfMeasureModel> Execute(int id)
        {
            var entity = await _repository.Query<domain.Data.UnitOfMeasure>()
                .Where(f => f.Id == id)
                .Select(f => new UnitOfMeasureModel
                {
                    Id = f.Id,
                    Name = f.Name,
                    IsActive = f.IsActive
                })
                .FirstOrDefaultAsync();

            if (entity == null)
                throw new ApplicationException("Unidad de medida no existe", "UNIDAD DE MEDIDA");

            return entity;
        }
    }
}
