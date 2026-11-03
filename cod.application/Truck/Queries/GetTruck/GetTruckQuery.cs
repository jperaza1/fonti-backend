using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Truck.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Truck.Queries.GetTruck
{
    public class GetTruckQuery : BaseQuery, IGetTruckQuery
    {
        public GetTruckQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<TruckModel> Execute(int id)
        {
            var truck = await _repository.Query<domain.Data.Truck>()
                .Where(f => f.Id == id)
                .Select(f => new TruckModel
                {
                    Id = f.Id,
                    Name = f.Name,
                    Plate = f.Plate,
                    TruckStatus = f.TruckStatus,
                    TruckStatusName = f.TruckStatus.GetEnumDescription(),
                    IsActive = f.IsActive,
                    RouteId = f.RouteId,
                    RouteName = f.RouteId.HasValue ? f.Route.Name : string.Empty,
                }).FirstOrDefaultAsync();

            if (truck == null)
                throw new ApplicationException("Camión no existe", "CAMION");

            return truck;
        }
    }
}
