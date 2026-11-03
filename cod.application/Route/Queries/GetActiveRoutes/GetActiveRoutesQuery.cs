using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Route.Common;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.Route.Queries.GetActiveRoutes
{
    public class GetActiveRoutesQuery : BaseQuery, IGetActiveRoutesQuery
    {
        private readonly AuthService _authService;

        public GetActiveRoutesQuery(IRepository repository, AuthService authService) : base(repository)
        {
            _authService = authService;
        }

        public async Task<List<RouteModel>> Execute()
        {
            var routes = _authService.Routes ?? new List<int>();
            var query = _repository.QueryActive<domain.Data.Route>();

            if (routes.Any())
                query = query.Where(route => routes.Contains(route.Id));

            return await query
                .OrderBy(route => route.Order)
                .Select(route => new RouteModel
                {
                    Id = route.Id,
                    Name = route.Name,
                    IsActive = route.IsActive,
                    TruckId = route.Truck != null ? route.Truck.Id : null,
                    Truck = route.Truck != null ? route.Truck.Plate : string.Empty
                }).ToListAsync();
        }
    }
}
