using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Production.Common;
using cod.domain.Features.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.Production.Queries.GetProductionShift
{
    public interface IGetProductionShiftQuery
    {
        Task<ProductionShiftModel> Execute(DateTime date, int shiftTypeId);
    }

    public class GetProductionShiftQuery : BaseQuery, IGetProductionShiftQuery
    {
        public GetProductionShiftQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<ProductionShiftModel> Execute(DateTime date, int shiftTypeId)
        {
            var day = date.Date;
            var nextDay = day.AddDays(1);

            var shift = await _repository.Query<domain.Operation.ProductionShift>()
                .Where(s => s.ShiftTypeId == shiftTypeId && s.ProductionDate >= day && s.ProductionDate < nextDay)
                .Select(s => new ProductionShiftModel
                {
                    Id = s.Id,
                    ProductionDate = s.ProductionDate,
                    ShiftTypeId = s.ShiftTypeId,
                    Shift = s.ShiftType.Name,
                    IsActive = s.IsActive,
                    IsClosed = s.IsClosed,
                    Status = !s.IsActive ? "Anulado" : s.IsClosed ? "Cerrado" : "Abierto"
                })
                .FirstOrDefaultAsync();

            if (shift == null)
                return null;

            var lines = await _repository.Query<domain.Operation.ProductionOrder>()
                .Where(o => o.ProductionShiftId == shift.Id)
                .OrderBy(o => o.Id)
                .Select(o => new ProductionLineModel
                {
                    Id = o.Id,
                    ProductId = o.ProductId,
                    Product = o.Product.Name,
                    Quantity = o.Quantity,
                    Note = o.Note,
                    IsActive = o.IsActive,
                    Status = o.IsActive ? "Vigente" : "Anulada"
                })
                .ToListAsync();

            var lineIds = lines.Select(l => l.Id).ToList();
            if (lineIds.Count > 0)
            {
                var losses = await _repository.Query<domain.Operation.InventoryKardex>()
                    .Where(k => k.MovementType == KardexMovementType.Loss
                        && k.ProductionOrderId != null
                        && lineIds.Contains(k.ProductionOrderId.Value))
                    .Select(k => new
                    {
                        k.ProductionOrderId,
                        Product = k.Product.Name,
                        Unit = k.Product.UnitOfMeasure.Name,
                        k.Quantity
                    })
                    .ToListAsync();

                foreach (var line in lines)
                {
                    var lineLosses = losses.Where(l => l.ProductionOrderId == line.Id).ToList();
                    if (lineLosses.Count == 0)
                        continue;

                    line.Losses = string.Join(", ", lineLosses.Select(l =>
                        l.Product + " " + l.Quantity.ToString("0.######") + (string.IsNullOrEmpty(l.Unit) ? "" : " " + l.Unit)));
                }
            }

            shift.Lines = lines;
            shift.Products = string.Join(", ", lines.Where(l => l.IsActive).Select(l => l.Product + " " + l.Quantity.ToString("0.######")));
            return shift;
        }
    }
}
