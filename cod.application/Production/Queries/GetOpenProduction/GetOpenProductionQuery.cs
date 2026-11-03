using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Production.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.Production.Queries.GetOpenProduction
{
    public interface IGetOpenProductionQuery
    {
        Task<ProductionShiftModel> Execute();
    }

    public class GetOpenProductionQuery : BaseQuery, IGetOpenProductionQuery
    {
        public GetOpenProductionQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<ProductionShiftModel> Execute()
        {
            return await _repository.WhereAsync<domain.Operation.ProductionShift>(s => s.IsActive && !s.IsClosed)
                .OrderByDescending(s => s.ProductionDate)
                .ThenByDescending(s => s.Id)
                .Select(s => new ProductionShiftModel
                {
                    Id = s.Id,
                    ProductionDate = s.ProductionDate,
                    ShiftTypeId = s.ShiftTypeId,
                    Shift = s.ShiftType.Name,
                    IsActive = s.IsActive,
                    IsClosed = s.IsClosed,
                    Status = "Abierto"
                })
                .FirstOrDefaultAsync();
        }
    }
}
