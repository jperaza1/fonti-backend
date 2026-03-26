using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Sale.Common;
using cod.domain.Operation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    f.SaleDate,
                    f.SaleType,
                    f.Amount,
                    f.IsActive,
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

            return await baseQuery.Select(f => new SaleModel
            {
                Id = f.Id,
                Amount = f.Amount,
                Route = f.Route,
                RouteId = f.RouteId,
                SaleDate = f.SaleDate,
                SaleType = f.SaleType,
                SaleTypeName = f.SaleType.GetEnumDescription(),
                IsActive = f.IsActive,
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
        }
    }
}
