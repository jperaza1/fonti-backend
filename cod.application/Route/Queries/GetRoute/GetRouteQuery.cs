using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Route.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Route.Queries.GetRoute
{
    public class GetRouteQuery : BaseQuery, IGetRouteQuery
    {
        public GetRouteQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<RouteModel> Execute(int id)
        {
            var route = await _repository.Query<domain.Data.Route>()
                .Where(f => f.Id == id)
                .Select(f => new RouteModel
                {
                    Id = f.Id,
                    Name = f.Name,
                    Vendor = f.Vendor,
                    IsActive = f.IsActive,
                    Order = f.Order,
                    DeliversRegalia = f.DeliversRegalia,
                    ProductRoutePrices = f.ProductRoutePrices.Select(g => new ProductRoutePricesModel
                    {
                        Id = g.Id,
                        Price = g.Price,
                        ProductId = g.ProductId,
                        ProductName = g.Product.Name
                    }).ToList()
                }).FirstOrDefaultAsync();

            if (route == null)
                throw new ApplicationException("Ruta no existe", "RUTA");

            return route;
        }
    }
}
