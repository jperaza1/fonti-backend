using cod.application.Route.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Route.Queries.GetRoutes
{
    public interface IGetRoutesQuery
    {
        Task<List<RouteModel>> Execute();
    }
}
