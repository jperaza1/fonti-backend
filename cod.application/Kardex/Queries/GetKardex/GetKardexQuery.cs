using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Kardex.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.Kardex.Queries.GetKardex
{
    public class GetKardexQuery : BaseQuery, IGetKardexQuery
    {
        public GetKardexQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<KardexModel>> Execute(KardexFilterModel filter)
        {
            var query = _repository.QueryActive<domain.Operation.InventoryKardex>();

            if (filter != null)
            {
                if (filter.ProductId.HasValue && filter.ProductId > 0)
                    query = query.Where(f => f.ProductId == filter.ProductId);

                if (filter.DateFrom.HasValue)
                {
                    var start = filter.DateFrom.Value.Date;
                    query = query.Where(f => f.MovementDate >= start);
                }

                if (filter.DateTo.HasValue)
                {
                    var end = filter.DateTo.Value.Date.AddDays(1);
                    query = query.Where(f => f.MovementDate < end);
                }
            }

            var rows = await query
                .OrderBy(f => f.MovementDate)
                .ThenBy(f => f.Id)
                .Select(f => new
                {
                    f.Id,
                    f.ProductId,
                    Product = f.Product.Name,
                    UnitOfMeasure = f.Product.UnitOfMeasure.Name,
                    f.MovementDate,
                    f.MovementType,
                    f.Quantity,
                    f.Balance,
                    f.UnitCost,
                    f.DispatchControlId,
                    f.SourceProductId,
                    SourceProduct = f.SourceProduct.Name,
                    f.Note
                })
                .ToListAsync();

            return rows.Select(f => new KardexModel
            {
                Id = f.Id,
                ProductId = f.ProductId,
                Product = f.Product,
                UnitOfMeasure = f.UnitOfMeasure,
                MovementDate = f.MovementDate,
                MovementType = f.MovementType,
                MovementTypeName = f.MovementType.GetEnumDescription(),
                Quantity = f.Quantity,
                Balance = f.Balance,
                UnitCost = f.UnitCost,
                DispatchControlId = f.DispatchControlId,
                SourceProductId = f.SourceProductId,
                SourceProduct = f.SourceProduct,
                Note = f.Note
            }).ToList();
        }
    }
}
