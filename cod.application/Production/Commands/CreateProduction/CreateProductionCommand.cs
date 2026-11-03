using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Inventory.Services;
using cod.application.Production.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Production.Commands.CreateProduction
{
    public interface ICreateProductionCommand
    {
        Task Execute(CreateProductionModel model);
    }

    public class CreateProductionCommand : BaseCommand, ICreateProductionCommand
    {
        private readonly IInventoryMovementService _inventoryMovementService;

        public CreateProductionCommand(
            IRepository repository,
            IExceptionFactory exceptionFactory,
            IInventoryMovementService inventoryMovementService) : base(repository, exceptionFactory)
        {
            _inventoryMovementService = inventoryMovementService;
        }

        public async Task Execute(CreateProductionModel model)
        {
            var shiftName = await Validate(model);
            var lines = FilledLines(model);
            var shift = await FindShift(model);

            if (shift != null && !shift.IsActive)
            {
                _exceptionFactory.Add("shiftTypeId", "Ese turno ya fue anulado y no se puede volver a abrir en esta fecha");
                throw new ApplicationException("CANCELED", "PRODUCCION", _exceptionFactory.Exceptions);
            }

            if (shift != null && shift.IsClosed)
            {
                _exceptionFactory.Add("shiftTypeId", "Ese turno ya está cerrado");
                throw new ApplicationException("CANCELED", "PRODUCCION", _exceptionFactory.Exceptions);
            }

            if (shift == null)
                await EnsureNoOtherOpenShift();

            if (shift == null)
            {
                shift = new domain.Operation.ProductionShift
                {
                    ProductionDate = model.ProductionDate.Date,
                    ShiftTypeId = model.ShiftTypeId
                };
                _repository.InsertAsync(shift);
            }

            var day = shift.ProductionDate.Date;
            var shiftTypeId = shift.ShiftTypeId;

            foreach (var line in lines)
            {
                var order = new domain.Operation.ProductionOrder
                {
                    ProductionDate = day,
                    ShiftTypeId = shiftTypeId,
                    ProductId = line.ProductId,
                    Quantity = line.Quantity,
                    Note = string.IsNullOrWhiteSpace(line.Note) ? null : line.Note.Trim()
                };

                if (shift.Id > 0)
                    order.ProductionShiftId = shift.Id;
                else
                    order.ProductionShift = shift;

                _repository.InsertAsync(order);
                await _inventoryMovementService.ApplyProduction(order, shiftName, ValidLosses(line));
            }

            await _repository.CommitAsync();
        }

        private async Task<domain.Operation.ProductionShift> FindShift(CreateProductionModel model)
        {
            if (model.ProductionShiftId > 0)
            {
                return await _repository.WhereAsync<domain.Operation.ProductionShift>(s => s.Id == model.ProductionShiftId)
                    .FirstOrDefaultAsync();
            }

            var day = model.ProductionDate.Date;
            var nextDay = day.AddDays(1);
            var sameDay = await _repository.WhereAsync<domain.Operation.ProductionShift>(s =>
                    s.ShiftTypeId == model.ShiftTypeId
                    && s.ProductionDate >= day
                    && s.ProductionDate < nextDay)
                .FirstOrDefaultAsync();

            if (sameDay != null)
                return sameDay;

            return await _repository.WhereAsync<domain.Operation.ProductionShift>(s =>
                    s.IsActive
                    && !s.IsClosed
                    && s.ShiftTypeId == model.ShiftTypeId)
                .OrderByDescending(s => s.ProductionDate)
                .ThenByDescending(s => s.Id)
                .FirstOrDefaultAsync();
        }

        private async Task EnsureNoOtherOpenShift()
        {
            var open = await _repository.WhereAsync<domain.Operation.ProductionShift>(s => s.IsActive && !s.IsClosed)
                .Select(s => new
                {
                    s.Id,
                    s.ProductionDate,
                    Name = s.ShiftType.Name
                })
                .FirstOrDefaultAsync();

            if (open == null)
                return;

            _exceptionFactory.Add(
                "shiftTypeId",
                "Hay un turno abierto (" + open.Name + ", " + open.ProductionDate.ToString("dd/MM/yyyy") + "). Ciérralo antes de abrir otro");
            throw new ApplicationException("CANCELED", "PRODUCCION", _exceptionFactory.Exceptions);
        }

        private static List<CreateProductionLineModel> FilledLines(CreateProductionModel model)
        {
            return (model.Lines ?? new List<CreateProductionLineModel>())
                .Where(l => l != null && (l.ProductId > 0 || l.Quantity > 0 || !string.IsNullOrWhiteSpace(l.Note) || HasLoss(l)))
                .ToList();
        }

        private static bool HasLoss(CreateProductionLineModel line)
        {
            return line.Losses != null && line.Losses.Any(l => l != null && (l.ProductId > 0 || l.Quantity > 0 || !string.IsNullOrWhiteSpace(l.Note)));
        }

        private static List<ProductionLossModel> ValidLosses(CreateProductionLineModel line)
        {
            return (line.Losses ?? new List<ProductionLossModel>())
                .Where(l => l != null && l.ProductId > 0 && l.Quantity > 0)
                .ToList();
        }

        private async Task<string> Validate(CreateProductionModel model)
        {
            if (model == null)
                throw new ApplicationException("CANCELED", "PRODUCCION", _exceptionFactory.Exceptions);

            if (model.ProductionDate == DateTime.MinValue)
                _exceptionFactory.Add("productionDate", "La fecha es requerida");

            if (model.ShiftTypeId <= 0)
                _exceptionFactory.Add("shiftTypeId", "El turno es requerido");

            var lines = model == null ? new List<CreateProductionLineModel>() : FilledLines(model);
            if (lines.Count == 0)
                _exceptionFactory.Add("lines", "Agrega al menos una línea de producto");

            if (!_exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "PRODUCCION", _exceptionFactory.Exceptions);

            var shift = await _repository.QueryActive<domain.Data.Type>()
                .Where(t => t.Id == model.ShiftTypeId)
                .Select(t => new { t.Name, GroupCode = t.TypeGroup.Code })
                .FirstOrDefaultAsync();

            if (shift == null || shift.GroupCode != ProductionShift.GroupCode)
                _exceptionFactory.Add("shiftTypeId", "El turno no es válido");

            var productIds = lines.Where(l => l.ProductId > 0).Select(l => l.ProductId).Distinct().ToList();
            var products = productIds.Count == 0
                ? new List<int>()
                : await _repository.QueryActive<domain.Data.Product>()
                    .Where(p => productIds.Contains(p.Id))
                    .Select(p => p.Id)
                    .ToListAsync();

            var recipes = productIds.Count == 0
                ? new List<int>()
                : await _repository.QueryActive<domain.Data.ProductRecipe>()
                    .Where(r => productIds.Contains(r.ProductId))
                    .Select(r => r.ProductId)
                    .ToListAsync();

            foreach (var line in lines)
            {
                if (line.ProductId <= 0)
                    _exceptionFactory.Add("lines", "El producto de la línea es requerido");
                else if (!products.Contains(line.ProductId))
                    _exceptionFactory.Add("lines", "El producto no existe");
                else if (!recipes.Contains(line.ProductId))
                    _exceptionFactory.Add("lines", "El producto no tiene receta activa");

                if (line.Quantity <= 0)
                    _exceptionFactory.Add("lines", "La cantidad de la línea debe ser mayor a cero");

                await ValidateLosses(line.Losses);
            }

            if (!_exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "PRODUCCION", _exceptionFactory.Exceptions);

            return shift.Name;
        }

        private async Task ValidateLosses(List<ProductionLossModel> source)
        {
            var losses = (source ?? new List<ProductionLossModel>())
                .Where(l => l != null && l.Quantity > 0)
                .ToList();

            if (losses.Count == 0)
                return;

            foreach (var loss in losses)
            {
                if (loss.ProductId <= 0)
                    _exceptionFactory.Add("losses", "La materia prima de la pérdida es requerida");
            }

            if (losses.Where(l => l.ProductId > 0).GroupBy(l => l.ProductId).Any(g => g.Count() > 1))
                _exceptionFactory.Add("losses", "La materia prima está repetida en las pérdidas");

            if (!_exceptionFactory.IsValid())
                return;

            var ids = losses.Select(l => l.ProductId).Distinct().ToList();
            var materials = await _repository.QueryActive<domain.Data.Product>()
                .Where(p => ids.Contains(p.Id))
                .Select(p => new { p.Id, p.ProductType })
                .ToListAsync();

            if (materials.Count != ids.Count || materials.Any(p => p.ProductType != domain.Features.Enums.ProductType.RawMaterial))
                _exceptionFactory.Add("losses", "Las pérdidas solo pueden ser materia prima activa");
        }
    }
}
