using cod.application.Common.Clases;
using cod.application.Common.Extensions;
using cod.application.DispatchControl.Commom;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.DispatchControl.Queries.GetAllDispatchControl
{
    public class GetAllDispatchControlQuery : BaseQuery, IGetAllDispatchControlQuery
    {
        private readonly AuthService _authService;
        public GetAllDispatchControlQuery(
            IRepository repository, AuthService authService) : base(repository)
        {
            _authService = authService;
        }

        public async Task<DispatchControlListModel> Execute(DispatchControlFilterModel filterModel)
        {
            filterModel ??= new DispatchControlFilterModel();
            var timezone = _authService.ClientTimezone;

            Expression<Func<domain.Operation.DispatchControl, bool>> byDate = GetDispatchControlByDate(filterModel, timezone);

            var rows = await _repository.Query<domain.Operation.DispatchControl>()
                .Where(byDate)
                .Where(GetDispatchControlByRoute(filterModel))
                .Select(f => new
                {
                    f.Id,
                    f.RouteId,
                    RouteName = f.Route.Name,
                    f.Vendor,
                    f.TruckId,
                    Truck = f.TruckId.HasValue ? f.Truck.Plate : string.Empty,
                    f.DispatchControlDate,
                    f.CreationTime,
                    f.CreatorUser,
                    f.Status,
                    Details = f.DispatchControlDetails
                        .Where(d => d.IsActive)
                        .Select(d => new
                        {
                            d.DispatchControlType,
                            Movements = d.ProductMovements
                                .Where(p => p.IsActive)
                                .Select(p => new
                                {
                                    p.ProductId,
                                    Product = p.Product.Name,
                                    p.Quantity
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToListAsync();

            var items = new List<DispatchControlModel>();
            foreach (var row in rows)
            {
                var lines = new List<ProductLine>();
                if (row.Details != null)
                {
                    foreach (var detail in row.Details)
                    {
                        if (detail.Movements == null)
                            continue;

                        foreach (var movement in detail.Movements)
                        {
                            lines.Add(new ProductLine
                            {
                                Type = detail.DispatchControlType,
                                ProductId = movement.ProductId,
                                Product = movement.Product,
                                Quantity = movement.Quantity
                            });
                        }
                    }
                }

                items.Add(new DispatchControlModel
                {
                    Id = row.Id,
                    DispatchControlDate = row.DispatchControlDate,
                    Route = row.RouteName,
                    RouteId = row.RouteId,
                    Vendor = row.Vendor,
                    TruckId = row.TruckId,
                    Truck = row.Truck,
                    CreationTime = row.CreationTime,
                    CreatorUser = row.CreatorUser,
                    Status = row.Status,
                    StatusName = row.Status.GetEnumDescription(),
                    ProductTotals = ProductTotals(lines, row.Status == DispatchControlStatus.Close)
                });
            }

            return new DispatchControlListModel
            {
                Items = items
            };
        }

        private static List<DispatchProductTotalModel> ProductTotals(IEnumerable<ProductLine> lines, bool closed)
        {
            var lineList = lines.ToList();
            var hasReturn = closed || lineList.Any(line => line.Type == DispatchControlType.Return);
            var totals = new Dictionary<int, DispatchProductTotalModel>();
            foreach (var line in lineList)
            {
                if (line.Quantity == 0)
                    continue;

                if (!totals.TryGetValue(line.ProductId, out var total))
                {
                    total = new DispatchProductTotalModel
                    {
                        ProductId = line.ProductId,
                        Product = line.Product ?? string.Empty
                    };
                    totals[line.ProductId] = total;
                }

                if (string.IsNullOrWhiteSpace(total.Product) && !string.IsNullOrWhiteSpace(line.Product))
                    total.Product = line.Product;

                switch (line.Type)
                {
                    case DispatchControlType.Exit:
                    case DispatchControlType.Reload:
                    case DispatchControlType.TransferIn:
                        total.Dispatched += line.Quantity;
                        break;
                    case DispatchControlType.TransferOut:
                        total.Dispatched -= line.Quantity;
                        break;
                    case DispatchControlType.Return:
                        total.Returned += line.Quantity;
                        break;
                }
            }

            foreach (var total in totals.Values)
            {
                if (hasReturn)
                    total.Sold = total.Dispatched - total.Returned;
            }

            return totals.Values
                .Where(total => total.Dispatched != 0 || total.Returned != 0 || total.Sold.GetValueOrDefault() != 0)
                .OrderBy(total => total.Product)
                .ToList();
        }

        public static Expression<Func<domain.Operation.DispatchControl, bool>> GetDispatchControlByDate(DispatchControlFilterModel filterModel, string timeZone)
        {
            if (filterModel.DateFrom.HasValue && filterModel.DateTo.HasValue)
            {
                filterModel.DateFrom = DateService.GetCLientLocalTime(filterModel.DateFrom.Value, timeZone);
                filterModel.DateTo = DateService.GetCLientLocalTime(filterModel.DateTo.Value, timeZone);
                return e => e.DispatchControlDate.Date >= filterModel.DateFrom.Value.Date && e.DispatchControlDate.Date <= filterModel.DateTo.Value.Date;
            }

            return e => true;
        }

        public static Expression<Func<domain.Operation.DispatchControl, bool>> GetDispatchControlByRoute(DispatchControlFilterModel filterModel)
        {
            if (filterModel.RouteId is null)
                return control => true;

            if (filterModel.RouteId > 0)
                return control => control.RouteId == filterModel.RouteId;

            return control => control.Route.Name == null || control.Route.Name.ToLower() != "planta";
        }

        private class ProductLine
        {
            public DispatchControlType Type { get; set; }
            public int ProductId { get; set; }
            public string Product { get; set; }
            public int Quantity { get; set; }
        }
    }
}
