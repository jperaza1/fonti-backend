using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Route.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Route.Queries.GetRoutes
{
    public class GetRoutesQuery : BaseQuery, IGetRoutesQuery
    {
        public GetRoutesQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<RouteModel>> Execute()
        {
            return await _repository.Query<domain.Data.Route>()
                .OrderBy(f => f.Order)
                .Select(f => new RouteModel
            {
                Id = f.Id,
                Name = f.Name,
                Vendor = f.Vendor,
                IsActive = f.IsActive,
                DeliversRegalia = f.DeliversRegalia,
                TruckId = f.Truck != null ? f.Truck.Id : null,
                Truck = f.Truck != null ? f.Truck.Plate : string.Empty
            }).ToListAsync();
        }
    }
}
