using cod.application.Route.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.Route.Queries.GetActiveRoutes
{
    public interface IGetActiveRoutesQuery
    {
        Task<List<RouteModel>> Execute();
    }
}
