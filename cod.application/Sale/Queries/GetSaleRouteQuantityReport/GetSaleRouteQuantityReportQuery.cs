using cod.application.Common.Clases;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.Sale.Queries.GetSaleRouteQuantityReport
{
    public class GetSaleRouteQuantityReportQuery : BaseQuery, IGetSaleRouteQuantityReportQuery
    {
        private readonly AuthService _authService;

        public GetSaleRouteQuantityReportQuery(IRepository repository, AuthService authService) : base(repository)
        {
            _authService = authService;
        }

        public async Task<SaleRouteQuantityReportModel> Execute(SaleRouteQuantityFilterModel filter)
        {
            filter = filter ?? new SaleRouteQuantityFilterModel();
            var today = Clock.Now;
            var from = filter.DateFrom ?? new DateTime(today.Year, today.Month, 1);
            var to = filter.DateTo ?? today;
            var timezone = _authService.ClientTimezone;
            from = DateService.GetCLientLocalTime(from, timezone).Date;
            to = DateService.GetCLientLocalTime(to, timezone).Date;
            if (from > to)
            {
                var swap = from;
                from = to;
                to = swap;
            }

            var routeIds = _authService.Routes ?? new List<int>();
            var sales = _repository.QueryActive<domain.Operation.Sale>()
                .Where(sale => sale.SaleDate.Date >= from && sale.SaleDate.Date <= to);
            if (routeIds.Any())
                sales = sales.Where(sale => routeIds.Contains(sale.RouteId));

            var lines = await sales
                .SelectMany(sale => sale.SaleDetails)
                .Where(detail => detail.IsActive)
                .Select(detail => new
                {
                    detail.Sale.RouteId,
                    Route = detail.Sale.Route.Name,
                    Vendor = detail.Sale.Route.Vendor,
                    RouteOrder = detail.Sale.Route.Order,
                    detail.ProductId,
                    Product = detail.Product.Name,
                    detail.Quantity
                })
                .ToListAsync();

            var products = lines
                .GroupBy(line => line.ProductId)
                .Select(group => new SaleRouteQuantityProductModel
                {
                    ProductId = group.Key,
                    Name = group.Select(line => line.Product).FirstOrDefault(name => !string.IsNullOrWhiteSpace(name)) ?? string.Empty
                })
                .OrderBy(product => product.Name)
                .ToList();

            var productIndex = new Dictionary<int, int>();
            for (var index = 0; index < products.Count; index++)
                productIndex[products[index].ProductId] = index;

            var routeQuery = _repository.QueryActive<domain.Data.Route>();
            if (routeIds.Any())
                routeQuery = routeQuery.Where(route => routeIds.Contains(route.Id));

            var activeRoutes = await routeQuery
                .Select(route => new
                {
                    route.Id,
                    route.Name,
                    route.Vendor,
                    route.Order
                })
                .ToListAsync();

            var heads = activeRoutes
                .Select(route => new RouteHead
                {
                    Id = route.Id,
                    Name = route.Name,
                    Vendor = route.Vendor,
                    Order = route.Order
                })
                .ToList();

            var known = new HashSet<int>(heads.Select(route => route.Id));
            foreach (var extra in lines.GroupBy(line => line.RouteId))
            {
                if (known.Contains(extra.Key))
                    continue;
                var sample = extra.First();
                heads.Add(new RouteHead
                {
                    Id = sample.RouteId,
                    Name = sample.Route,
                    Vendor = sample.Vendor,
                    Order = sample.RouteOrder
                });
            }

            heads.Sort((left, right) =>
            {
                var byOrder = left.Order.CompareTo(right.Order);
                return byOrder != 0
                    ? byOrder
                    : string.Compare(left.Name, right.Name, StringComparison.CurrentCultureIgnoreCase);
            });

            var quantities = new Dictionary<(int RouteId, int ProductId), int>();
            foreach (var line in lines)
            {
                var key = (line.RouteId, line.ProductId);
                quantities.TryGetValue(key, out var current);
                quantities[key] = current + line.Quantity;
            }

            var rows = new List<SaleRouteQuantityRowModel>();
            var totals = new int[products.Count];

            if (products.Count > 0)
            {
                foreach (var route in heads)
                {
                    var rowQuantities = new int[products.Count];
                    foreach (var product in products)
                    {
                        var index = productIndex[product.ProductId];
                        quantities.TryGetValue((route.Id, product.ProductId), out var quantity);
                        rowQuantities[index] = quantity;
                        totals[index] += quantity;
                    }

                    rows.Add(new SaleRouteQuantityRowModel
                    {
                    RouteId = route.Id,
                    Route = route.Name,
                    Vendor = route.Vendor,
                    Quantities = rowQuantities.ToList()
                    });
                }
            }

            return new SaleRouteQuantityReportModel
            {
                Products = products,
                Rows = rows,
                Totals = totals.ToList()
            };
        }

        private class RouteHead
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Vendor { get; set; }
            public int Order { get; set; }
        }
    }
}
