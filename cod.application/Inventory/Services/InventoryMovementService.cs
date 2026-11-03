using cod.application.Common.Extensions;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.ProductRecipe;
using cod.application.Production.Common;
using cod.domain.Features;
using cod.domain.Features.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.Inventory.Services
{
    public class InventoryMovementService : IInventoryMovementService
    {
        private readonly IRepository _repository;
        private readonly IExceptionFactory _exceptionFactory;
        private const string DispatchApplyStockKey = "DISPATCH_APPLY_STOCK";

        private readonly Dictionary<int, cod.domain.Operation.ProductStock> _stocks = new Dictionary<int, cod.domain.Operation.ProductStock>();
        private readonly Dictionary<int, (string Name, decimal? Cost)> _products = new Dictionary<int, (string Name, decimal? Cost)>();
        private bool? _dispatchAffectsStock;

        public InventoryMovementService(IRepository repository, IExceptionFactory exceptionFactory)
        {
            _repository = repository;
            _exceptionFactory = exceptionFactory;
        }

        public async Task ApplyDispatch(cod.domain.Operation.DispatchControl dispatch)
        {
            if (!await DispatchAffectsStock())
                return;

            if (dispatch.DispatchControlDetails == null)
                return;

            foreach (var detail in dispatch.DispatchControlDetails)
                await ApplyDetail(dispatch, detail);
        }

        public async Task ApplyDetail(cod.domain.Operation.DispatchControl dispatch, cod.domain.Operation.DispatchControlDetail detail)
        {
            if (!await DispatchAffectsStock())
                return;

            if (detail.ProductMovements == null || detail.ProductMovements.Count == 0)
                return;

            if (!TryGetMovement(detail.DispatchControlType, out var movementType, out var inbound))
                return;

            var movementDate = dispatch.DispatchControlDate == DateTime.MinValue
                ? Clock.Now
                : dispatch.DispatchControlDate;
            var typeName = movementType.GetEnumDescription();

            foreach (var movement in detail.ProductMovements.Where(p => p.Quantity > 0))
            {
                var product = await GetProduct(movement.ProductId);
                await ApplyLine(
                    movement.ProductId,
                    movement.Quantity,
                    inbound,
                    movementType,
                    movementDate,
                    typeName + " de " + product.Name,
                    dispatch,
                    null,
                    product.Cost,
                    null);
            }
        }

        public Task ApplyEntry(int productId, decimal quantity, DateTime movementDate, string note, decimal? unitCost)
        {
            return ApplyLine(productId, quantity, true, KardexMovementType.Entry, movementDate, note, null, null, unitCost, null);
        }

        public Task ApplyExit(int productId, decimal quantity, DateTime movementDate, string note, decimal? unitCost)
        {
            return ApplyLine(productId, quantity, false, KardexMovementType.Exit, movementDate, note, null, null, unitCost, null);
        }

        public Task ApplyQualityExit(int productId, decimal quantity, DateTime movementDate, string note, decimal? unitCost)
        {
            return ApplyLine(productId, quantity, false, KardexMovementType.QualityExit, movementDate, note, null, null, unitCost, null);
        }

        public async Task<List<ProductionConsumptionModel>> GetConsumption(int productId, decimal quantity)
        {
            var recipe = await LoadRecipe(productId);
            var lines = new List<ProductionConsumptionModel>();

            foreach (var line in recipe.Details.Where(d => d.IsActive))
            {
                var rawQuantity = InventoryQuantity.ForFinishedUnits(
                    quantity,
                    recipe.UnitsPerPackage,
                    line.Quantity,
                    line.ProducedUnits,
                    line.PerPackage);

                if (rawQuantity <= 0)
                    continue;

                lines.Add(new ProductionConsumptionModel
                {
                    ProductId = line.RawMaterialProductId,
                    Product = line.RawMaterialProduct?.Name,
                    UnitOfMeasure = line.RawMaterialProduct?.UnitOfMeasure?.Name,
                    Quantity = rawQuantity
                });
            }

            return lines;
        }

        public async Task ApplyProduction(cod.domain.Operation.ProductionOrder order, string shiftName, List<ProductionLossModel> losses)
        {
            var recipe = await LoadRecipe(order.ProductId);
            var product = await GetProduct(order.ProductId);
            var header = string.IsNullOrWhiteSpace(order.Note)
                ? "Producción de " + product.Name + (string.IsNullOrWhiteSpace(shiftName) ? "" : ", " + shiftName)
                : order.Note.Trim();

            foreach (var line in recipe.Details.Where(d => d.IsActive))
            {
                var rawQuantity = InventoryQuantity.ForFinishedUnits(
                    order.Quantity,
                    recipe.UnitsPerPackage,
                    line.Quantity,
                    line.ProducedUnits,
                    line.PerPackage);

                var rawMaterial = await GetProduct(line.RawMaterialProductId);
                await ApplyLine(
                    line.RawMaterialProductId,
                    rawQuantity,
                    false,
                    KardexMovementType.Production,
                    order.ProductionDate,
                    header + " (" + rawMaterial.Name + ")",
                    null,
                    order.ProductId,
                    rawMaterial.Cost,
                    order);
            }

            if (losses != null)
            {
                foreach (var loss in losses.Where(l => l != null && l.ProductId > 0 && l.Quantity > 0))
                {
                    var rawMaterial = await GetProduct(loss.ProductId);
                    var lossNote = "Pérdida de " + rawMaterial.Name + " en producción de " + product.Name;
                    if (!string.IsNullOrWhiteSpace(loss.Note))
                        lossNote += ". " + loss.Note.Trim();

                    await ApplyLine(
                        loss.ProductId,
                        loss.Quantity,
                        false,
                        KardexMovementType.Loss,
                        order.ProductionDate,
                        lossNote,
                        null,
                        order.ProductId,
                        rawMaterial.Cost,
                        order);
                }
            }

            await ApplyLine(
                order.ProductId,
                order.Quantity,
                true,
                KardexMovementType.Production,
                order.ProductionDate,
                header,
                null,
                null,
                product.Cost,
                order);
        }

        public async Task ReverseProduction(cod.domain.Operation.ProductionOrder order)
        {
            var lines = await _repository.WhereAsync<cod.domain.Operation.InventoryKardex>(
                    k => k.ProductionOrderId == order.Id
                        && (k.MovementType == KardexMovementType.Production || k.MovementType == KardexMovementType.Loss))
                .ToListAsync();

            var product = await GetProduct(order.ProductId);
            var note = "Anulación de producción de " + product.Name;
            var movementDate = Clock.Now;

            foreach (var line in lines.Where(l => l.SourceProductId == null))
            {
                await ApplyLine(
                    line.ProductId,
                    line.Quantity,
                    false,
                    KardexMovementType.Production,
                    movementDate,
                    note,
                    null,
                    null,
                    line.UnitCost,
                    order);
            }

            foreach (var line in lines.Where(l => l.SourceProductId != null))
            {
                await ApplyLine(
                    line.ProductId,
                    line.Quantity,
                    true,
                    KardexMovementType.Production,
                    movementDate,
                    note,
                    null,
                    line.SourceProductId,
                    line.UnitCost,
                    order);
            }
        }

        private async Task<domain.Data.ProductRecipe> LoadRecipe(int productId)
        {
            var recipe = await _repository.WhereAsync<domain.Data.ProductRecipe>(r => r.ProductId == productId && r.IsActive)
                .Include(r => r.Details)
                .ThenInclude(d => d.RawMaterialProduct)
                .ThenInclude(p => p.UnitOfMeasure)
                .FirstOrDefaultAsync();

            if (recipe == null || recipe.Details == null || !recipe.Details.Any(d => d.IsActive))
            {
                _exceptionFactory.Add("productId", "El producto no tiene receta activa");
                throw new ApplicationException("CANCELED", "PRODUCCION", _exceptionFactory.Exceptions);
            }

            return recipe;
        }

        private async Task ApplyLine(
            int productId,
            decimal quantity,
            bool inbound,
            KardexMovementType movementType,
            DateTime movementDate,
            string note,
            cod.domain.Operation.DispatchControl dispatch,
            int? sourceProductId,
            decimal? unitCost,
            cod.domain.Operation.ProductionOrder production)
        {
            quantity = InventoryQuantity.Round(quantity);
            if (quantity <= 0)
                return;

            var stock = await GetStock(productId);
            var available = InventoryQuantity.Round(stock.Quantity);

            if (!inbound && available < quantity)
            {
                var product = await GetProduct(productId);
                _exceptionFactory.Add(
                    "stock",
                    "No hay stock suficiente de " + product.Name
                    + ". Disponible: " + available.ToString(CultureInfo.InvariantCulture)
                    + ". Requerido: " + quantity.ToString(CultureInfo.InvariantCulture) + ".");
                throw new ApplicationException("CANCELED", "INVENTARIO", _exceptionFactory.Exceptions);
            }

            var balance = InventoryQuantity.Round(stock.Quantity + (inbound ? quantity : -quantity));
            stock.Quantity = balance;
            if (stock.Id > 0)
                _repository.UpdateSync(stock);

            var kardex = new cod.domain.Operation.InventoryKardex
            {
                ProductId = productId,
                MovementDate = movementDate,
                MovementType = movementType,
                Quantity = quantity,
                Balance = balance,
                UnitCost = unitCost,
                SourceProductId = sourceProductId,
                Note = note
            };

            if (dispatch != null)
            {
                if (dispatch.Id > 0)
                    kardex.DispatchControlId = dispatch.Id;
                else
                    kardex.DispatchControl = dispatch;
            }

            if (production != null)
            {
                if (production.Id > 0)
                    kardex.ProductionOrderId = production.Id;
                else
                    kardex.ProductionOrder = production;
            }

            _repository.InsertAsync(kardex);
        }

        private async Task<cod.domain.Operation.ProductStock> GetStock(int productId)
        {
            if (_stocks.TryGetValue(productId, out var cached))
                return cached;

            var stock = await _repository.WhereAsync<cod.domain.Operation.ProductStock>(s => s.ProductId == productId).FirstOrDefaultAsync();
            if (stock == null)
            {
                stock = new cod.domain.Operation.ProductStock
                {
                    ProductId = productId,
                    Quantity = 0
                };
                _repository.InsertAsync(stock);
            }

            _stocks[productId] = stock;
            return stock;
        }

        private async Task<(string Name, decimal? Cost)> GetProduct(int productId)
        {
            if (_products.TryGetValue(productId, out var cached))
                return cached;

            var product = await _repository.Query<domain.Data.Product>()
                .Where(p => p.Id == productId)
                .Select(p => new { p.Name, p.Cost })
                .FirstOrDefaultAsync();

            var value = product == null
                ? ("Producto " + productId, (decimal?)null)
                : (product.Name, product.Cost);

            _products[productId] = value;
            return value;
        }

        /// <summary>
        /// gen.Configurations, clave DISPATCH_APPLY_STOCK. Solo "true" mueve inventario.
        /// Si no existe, se crea en false para poder activarla en Company Settings.
        /// </summary>
        private async Task<bool> DispatchAffectsStock()
        {
            if (_dispatchAffectsStock.HasValue)
                return _dispatchAffectsStock.Value;

            var configuration = await _repository
                .QueryActive<domain.General.Configuration>(c => c.Key == DispatchApplyStockKey)
                .Select(c => new { c.Value })
                .FirstOrDefaultAsync();

            if (configuration == null)
            {
                var exists = await _repository.AnyAsync<domain.General.Configuration>(
                    c => c.Key == DispatchApplyStockKey);
                if (!exists)
                {
                    _repository.InsertAsync(new domain.General.Configuration
                    {
                        Key = DispatchApplyStockKey,
                        Value = "false"
                    });
                }

                _dispatchAffectsStock = false;
                return false;
            }

            _dispatchAffectsStock = string.Equals(
                configuration.Value?.Trim(),
                "true",
                StringComparison.OrdinalIgnoreCase);
            return _dispatchAffectsStock.Value;
        }

        private static bool TryGetMovement(DispatchControlType dispatchControlType, out KardexMovementType movementType, out bool inbound)
        {
            switch (dispatchControlType)
            {
                case DispatchControlType.Exit:
                    movementType = KardexMovementType.Exit;
                    inbound = false;
                    return true;
                case DispatchControlType.Reload:
                    movementType = KardexMovementType.Reload;
                    inbound = false;
                    return true;
                case DispatchControlType.Return:
                    movementType = KardexMovementType.Return;
                    inbound = true;
                    return true;
                default:
                    movementType = default;
                    inbound = false;
                    return false;
            }
        }
    }
}
