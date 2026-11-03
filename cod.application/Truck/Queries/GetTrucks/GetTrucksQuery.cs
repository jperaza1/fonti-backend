using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Truck.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.Truck.Queries.GetTrucks
{
    public class GetTrucksQuery : BaseQuery, IGetTrucksQuery
    {
        public GetTrucksQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<TruckModel>> Execute()
        {
            return await _repository.Query<domain.Data.Truck>().Select(f => new TruckModel
            {
                Id = f.Id,
                Name = f.Name,
                Plate = f.Plate,
                TruckStatus = f.TruckStatus,
                TruckStatusName = f.TruckStatus.GetEnumDescription(),
                IsActive = f.IsActive,
                RouteId = f.RouteId,
                RouteName = f.RouteId.HasValue ? f.Route.Name : string.Empty,
            }).OrderBy(f => f.Name).ToListAsync();
        }
    }
}
