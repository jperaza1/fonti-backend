using cod.application.Route.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Route.Queries.GetRoute
{
    public interface IGetRouteQuery
    {
        Task<RouteModel> Execute(int id);
    }
}
