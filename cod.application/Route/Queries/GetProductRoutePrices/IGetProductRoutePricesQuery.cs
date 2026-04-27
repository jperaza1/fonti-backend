using cod.application.Route.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Route.Queries.GetProductRoutePrices
{
    public interface IGetProductRoutePricesQuery
    {
        Task<List<ProductRoutePricesModel>> Execute(int routeId);
    }
}
