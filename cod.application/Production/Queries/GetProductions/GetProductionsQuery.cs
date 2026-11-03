using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Production.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.Production.Queries.GetProductions
{
    public interface IGetProductionsQuery
    {
        Task<List<ProductionShiftModel>> Execute(ProductionFilterModel filter);
    }

    public class GetProductionsQuery : BaseQuery, IGetProductionsQuery
    {
        public GetProductionsQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<ProductionShiftModel>> Execute(ProductionFilterModel filter)
        {
            var query = _repository.Query<domain.Operation.ProductionShift>();

            if (filter != null)
            {
                if (filter.DateFrom.HasValue)
                {
                    var start = filter.DateFrom.Value.Date;
                    query = query.Where(f => f.ProductionDate >= start);
                }

                if (filter.DateTo.HasValue)
                {
                    var end = filter.DateTo.Value.Date.AddDays(1);
                    query = query.Where(f => f.ProductionDate < end);
                }
            }

            var shifts = await query
                .OrderByDescending(f => f.ProductionDate)
                .ThenBy(f => f.ShiftType.Name)
                .Select(f => new ProductionShiftModel
                {
                    Id = f.Id,
                    ProductionDate = f.ProductionDate,
                    ShiftTypeId = f.ShiftTypeId,
                    Shift = f.ShiftType.Name,
                    IsActive = f.IsActive,
                    IsClosed = f.IsClosed,
                    Status = !f.IsActive ? "Anulado" : f.IsClosed ? "Cerrado" : "Abierto"
                })
                .ToListAsync();

            if (shifts.Count == 0)
                return shifts;

            var ids = shifts.Select(s => s.Id).ToList();
            var lines = await _repository.Query<domain.Operation.ProductionOrder>()
                .Where(o => o.ProductionShiftId != null && ids.Contains(o.ProductionShiftId.Value) && o.IsActive)
                .Select(o => new
                {
                    o.ProductionShiftId,
                    Product = o.Product.Name,
                    o.Quantity
                })
                .ToListAsync();

            foreach (var shift in shifts)
            {
                var shiftLines = lines.Where(l => l.ProductionShiftId == shift.Id).ToList();
                shift.Products = shiftLines.Count == 0
                    ? ""
                    : string.Join(", ", shiftLines.Select(l => l.Product + " " + l.Quantity.ToString("0.######")));
            }

            return shifts;
        }
    }
}
