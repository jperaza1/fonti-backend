using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Sale.Commands.ConsolidateSale;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.Sale.Queries.GetConsolidateAvailability
{
    public class GetConsolidateAvailabilityQuery : BaseQuery, IGetConsolidateAvailabilityQuery
    {
        public GetConsolidateAvailabilityQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<ConsolidateAvailabilityLine>> Execute(ConsolidateSaleModel model)
        {
            var saleIds = (model?.SaleIds ?? new List<int>()).Where(id => id > 0).Distinct().ToList();
            if (model == null || model.RouteId <= 0 || !saleIds.Any())
                return new List<ConsolidateAvailabilityLine>();

            var rows = await _repository.Query<domain.Operation.SaleDetail>()
                .Where(detail => detail.IsActive
                    && detail.Quantity > 0
                    && detail.Price > 0
                    && saleIds.Contains(detail.SaleId)
                    && detail.Sale.IsActive
                    && detail.Sale.RouteId == model.RouteId
                    && detail.Sale.SaleType == SaleType.Cash
                    && !detail.Sale.SaleInvoiceId.HasValue)
                .Select(detail => new
                {
                    detail.ProductId,
                    Product = detail.Product.Name,
                    detail.Price,
                    detail.Quantity
                })
                .ToListAsync();

            return rows
                .GroupBy(detail => new { detail.ProductId, detail.Product, detail.Price })
                .Select(group => new ConsolidateAvailabilityLine
                {
                    ProductId = group.Key.ProductId,
                    Product = group.Key.Product,
                    Price = group.Key.Price,
                    Quantity = group.Sum(detail => detail.Quantity)
                })
                .OrderBy(line => line.Product)
                .ThenBy(line => line.Price)
                .ToList();
        }
    }
}
