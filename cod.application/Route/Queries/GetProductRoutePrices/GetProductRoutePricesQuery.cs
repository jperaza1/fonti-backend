using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Route.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Route.Queries.GetProductRoutePrices
{
    public class GetProductRoutePricesQuery : BaseQuery, IGetProductRoutePricesQuery
    {
        public GetProductRoutePricesQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<ProductRoutePricesModel>> Execute(int routeId)
        {
            var baseQuery = _repository.QueryActive<domain.Data.ProductRoutePrice>()
                .Where(f => f.RouteId == routeId);

            return await baseQuery.Select(g => new ProductRoutePricesModel
            {
                Id = g.Id,
                ProductId = g.ProductId,
                ProductName = g.Product.Name,
                Price = g.Price,
            })
                .OrderBy(f => f.ProductName)
                .OrderByDescending(f => f.Price)
                .ToListAsync();
        }
    }
}
