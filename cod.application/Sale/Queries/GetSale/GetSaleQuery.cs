using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Sale.Common;
using cod.domain.Operation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.Sale.Queries.GetSale
{
    public class GetSaleQuery : BaseQuery, IGetSaleQuery
    {
        public GetSaleQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<SaleModel> Execute(int id)
        {
            var baseQuery = _repository.Query<domain.Operation.Sale>()
                .Where(f => f.Id == id)
                .Select(f => new
            {
                    f.Id,
                    f.RouteId,
                    Route = f.Route.Name,
                    f.Vendor,
                    f.SaleDate,
                    f.SaleType,
                    f.Amount,
                    f.IsActive,
                    f.CustomerId,
                    f.SaleInvoiceId,
                    CustomerName = f.CustomerId.HasValue ? f.Customer.Name : string.Empty,
                    SaleDetails = f.SaleDetails.Select(g => new
                    {
                        g.Id,
                        g.SaleId,
                        g.ProductId,
                        Product = g.Product.Name,
                        g.Price,
                        g.Quantity,
                        g.Total
                    })
                });

            var sale = await baseQuery.Select(f => new SaleModel
            {
                Id = f.Id,
                Amount = f.Amount,
                Route = f.Route,
                RouteId = f.RouteId,
                Vendor = f.Vendor,
                SaleDate = f.SaleDate,
                SaleType = f.SaleType,
                SaleTypeName = f.SaleType.GetEnumDescription(),
                IsActive = f.IsActive,
                CustomerId = f.CustomerId,
                CustomerName = f.CustomerName,
                SaleInvoiceId = f.SaleInvoiceId,
                SaleDetails = f.SaleDetails.Select(g => new SaleDetailModel
                {
                    Id = g.Id,
                    Price = g.Price,
                    Product = g.Product,
                    ProductId = g.ProductId,
                    Quantity = g.Quantity,
                    Total = g.Total,
                    SaleId = g.SaleId,
                }).ToList()
            }).FirstOrDefaultAsync();

            if (sale == null)
                return null;

            sale.DispatchSummary = await SoldByDispatch(sale.RouteId, sale.SaleDate);
            sale.HasDispatchControl = sale.DispatchSummary != null;
            if (sale.DispatchSummary == null)
                sale.DispatchSummary = new List<SaleDispatchProductModel>();

            return sale;
        }

        private async Task<List<SaleDispatchProductModel>> SoldByDispatch(int routeId, DateTime saleDate)
        {
            var details = await _repository.QueryActive<domain.Operation.DispatchControl>()
                .Where(control => control.RouteId == routeId && control.DispatchControlDate.Date == saleDate.Date)
                .OrderByDescending(control => control.Id)
                .Select(control => control.DispatchControlDetails
                    .Where(detail => detail.IsActive)
                    .Select(detail => new
                    {
                        detail.Id,
                        detail.DispatchControlType,
                        Movements = detail.ProductMovements
                            .Where(movement => movement.IsActive)
                            .Select(movement => new
                            {
                                movement.ProductId,
                                Name = movement.Product.Name,
                                movement.Quantity
                            })
                            .ToList()
                    })
                    .ToList())
                .FirstOrDefaultAsync();

            if (details == null)
                return null;

            details = details
                .OrderBy(detail => detail.DispatchControlType)
                .ThenBy(detail => detail.Id)
                .ToList();

            var order = new List<int>();
            var names = new Dictionary<int, string>();
            var sold = new Dictionary<int, int>();

            foreach (var detail in details)
            {
                var sign = detail.DispatchControlType == DispatchControlType.Return ? -1 : 1;
                foreach (var movement in detail.Movements)
                {
                    if (!names.ContainsKey(movement.ProductId))
                    {
                        names[movement.ProductId] = movement.Name;
                        order.Add(movement.ProductId);
                        sold[movement.ProductId] = 0;
                    }

                    sold[movement.ProductId] += sign * movement.Quantity;
                }
            }

            return order.Select(productId => new SaleDispatchProductModel
            {
                Product = names[productId],
                Quantity = sold[productId]
            }).ToList();
        }
    }
}
