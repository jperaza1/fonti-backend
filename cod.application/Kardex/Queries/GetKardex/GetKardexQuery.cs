using cod.application.Common.Clases;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Kardex.Common;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.Kardex.Queries.GetKardex
{
    public class GetKardexQuery : BaseQuery, IGetKardexQuery
    {
        private readonly AuthService _authService;
        public GetKardexQuery(IRepository repository, AuthService authService) : base(repository)
        {
            _authService = authService;
        }

        public async Task<List<KardexModel>> Execute(KardexFilterModel filter)
        {
            var query = _repository.QueryActive<domain.Operation.InventoryKardex>();

            var timezone = _authService.ClientTimezone;



            if (filter != null)
            {
                if (filter.ProductId.HasValue && filter.ProductId > 0)
                    query = query.Where(f => f.ProductId == filter.ProductId);

                if (filter.WarehouseId.HasValue && filter.WarehouseId > 0)
                    query = query.Where(f => f.WarehouseId == filter.WarehouseId);

                if (filter.DateFrom.HasValue)
                {
                    var start = DateService.GetCLientLocalTime(filter.DateFrom.Value, timezone);
                    query = query.Where(f => f.MovementDate.Date >= start.Date);
                }

                if (filter.DateTo.HasValue)
                {
                    var end = DateService.GetCLientLocalTime(filter.DateTo.Value, timezone);
                    query = query.Where(f => f.MovementDate.Date <= end.Date);
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
                    f.WarehouseId,
                    Warehouse = f.Warehouse.Name,
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
                WarehouseId = f.WarehouseId,
                Warehouse = f.Warehouse,
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
