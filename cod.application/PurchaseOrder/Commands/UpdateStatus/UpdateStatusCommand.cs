using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Inventory.Services;
using cod.domain.Features;
using cod.domain.Features.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.PurchaseOrder.Commands.UpdateStatus
{
    public class UpdateStatusCommand : BaseCommand, IUpdateStatusCommand
    {
        public const string PriceApprovalKey = "PURCHASE_ORDER_PRICE_APPROVAL";

        private readonly AuthService _authService;
        private readonly IInventoryMovementService _inventory;

        public UpdateStatusCommand(
            IRepository repository,
            IExceptionFactory exceptionFactory,
            AuthService authService,
            IInventoryMovementService inventory) : base(repository, exceptionFactory)
        {
            _authService = authService;
            _inventory = inventory;
        }

        public async Task Execute(int id)
        {
            var purchaseOrder = await _repository.QueryActive<domain.Operation.PurchaseOrder>()
                .Where(item => item.Id == id)
                .FirstOrDefaultAsync();

            if (purchaseOrder == null)
                throw new ApplicationException("Orden de compra no existe", "ORDEN COMPRA");

            await ValidateReception(id, purchaseOrder.OrderStatus);

            switch (purchaseOrder.OrderStatus)
            {
                case PurchaseOrderStatus.Created:
                    purchaseOrder.OrderStatus = PurchaseOrderStatus.Approved;
                    purchaseOrder.ApprovedAt = Clock.Now;
                    purchaseOrder.ApprovedUser = _authService.User;
                    break;
                case PurchaseOrderStatus.Approved:
                    purchaseOrder.OrderStatus = PurchaseOrderStatus.Processing;
                    purchaseOrder.ProcessingAt = Clock.Now;
                    purchaseOrder.ProcessingUser = _authService.User;
                    break;
                case PurchaseOrderStatus.Processing:
                    purchaseOrder.OrderStatus = PurchaseOrderStatus.Review;
                    purchaseOrder.ReviewedAt = Clock.Now;
                    purchaseOrder.ReviewedUser = _authService.User;
                    break;
                case PurchaseOrderStatus.Review:
                    purchaseOrder.OrderStatus = PurchaseOrderStatus.Completed;
                    purchaseOrder.CompletedAt = Clock.Now;
                    purchaseOrder.CompletedUser = _authService.User;
                    break;
                default:
                    throw new ApplicationException("La orden no puede cambiar de estado", "ORDEN COMPRA");
            }

            if (purchaseOrder.OrderStatus == PurchaseOrderStatus.Review)
            {
                await ReceiveInventory(id, purchaseOrder.Reference);
                await ApplyPriceChange(id);
            }

            _repository.UpdateSync(purchaseOrder);
            _repository.InsertAsync(new domain.General.DataChangeLog
            {
                EntityId = id,
                Entity = "PURCHASE_ORDER",
                Date = Clock.Now,
                Log = "Estado: " + purchaseOrder.OrderStatus.ToString()
            });

            await _repository.CommitAsync();
        }

        private async Task ValidateReception(int id, PurchaseOrderStatus status)
        {
            if (status != PurchaseOrderStatus.Processing && status != PurchaseOrderStatus.Review)
                return;

            var pending = await _repository.QueryActive<domain.Operation.PurchaseOrderDetail>()
                .Where(item => item.PurchaseOrderId == id)
                .AnyAsync(item => item.DetailStatus == DetailStatus.Pending || item.DetailStatus == DetailStatus.Partially_Received);

            if (pending)
                throw new ApplicationException("Hay productos pendientes o recibidos parcialmente", "ORDEN COMPRA");
        }

        private async Task ReceiveInventory(int id, string reference)
        {
            var lines = await _repository.QueryActive<domain.Operation.PurchaseOrderDetail>()
                .Where(item => item.PurchaseOrderId == id
                    && item.ProductVendorId.HasValue
                    && item.DetailStatus != DetailStatus.Pending
                    && item.DetailStatus != DetailStatus.Out_Of_Stock)
                .Select(item => new
                {
                    item.ProductVendor.ProductId,
                    item.ProductVendor.Unit,
                    item.QuantityRecived,
                    item.CostRecived,
                    item.Cost,
                    Name = item.ProductVendor.Product.Name
                })
                .ToListAsync();

            foreach (var line in lines)
            {
                var units = line.Unit <= 0 ? 1 : line.Unit;
                var quantity = (decimal)line.QuantityRecived * units;
                var packageCost = line.CostRecived > 0 ? line.CostRecived : line.Cost;
                var unitCost = (decimal)packageCost / units;
                await _inventory.ApplyEntry(
                    line.ProductId,
                    quantity,
                    Clock.Now,
                    "Recepción de orden " + reference + " (" + line.Name + ")",
                    unitCost);
            }
        }

        private async Task ApplyPriceChange(int id)
        {
            var lines = await _repository.QueryActive<domain.Operation.PurchaseOrderDetail>()
                .Where(item => item.PurchaseOrderId == id && item.ProductVendorId.HasValue)
                .Where(item => item.DetailStatus == DetailStatus.Price_Changed || item.DetailStatus == DetailStatus.Prince_Quantity_Mismatch)
                .Select(item => new
                {
                    ProductVendorId = item.ProductVendorId.Value,
                    item.Cost,
                    item.CostRecived,
                    item.ProductVendor.ProductId,
                    item.ProductVendor.Unit
                })
                .ToListAsync();

            if (!lines.Any())
                return;

            if (await PriceApprovalEnabled())
            {
                var request = new domain.Operation.PriceApproval
                {
                    PurchaseOrderId = id,
                    DatePriceApproval = Clock.Now,
                    Status = PriceApprovalStatus.Pending,
                    Token = Guid.NewGuid().ToString("N"),
                    PriceApprovalDetails = lines.Select(item => new domain.Operation.PriceApprovalDetail
                    {
                        ProductVendorId = item.ProductVendorId,
                        OldCost = (decimal)item.Cost,
                        NewCost = (decimal)item.CostRecived
                    }).ToList()
                };

                foreach (var detail in request.PriceApprovalDetails)
                    _repository.SetAuthValuesToCreate(detail);

                _repository.InsertAsync(request);
                return;
            }

            foreach (var item in lines)
                await VendorPrice.Apply(_repository, item.ProductVendorId, item.ProductId, item.Unit, (decimal)item.CostRecived, "Orden de compra " + id);
        }

        private async Task<bool> PriceApprovalEnabled()
        {
            var configuration = await _repository
                .QueryActive<domain.General.Configuration>(item => item.Key == PriceApprovalKey)
                .Select(item => item.Value)
                .FirstOrDefaultAsync();

            if (configuration == null)
            {
                var exists = await _repository.AnyAsync<domain.General.Configuration>(item => item.Key == PriceApprovalKey);
                if (!exists)
                {
                    _repository.InsertAsync(new domain.General.Configuration
                    {
                        Key = PriceApprovalKey,
                        Value = "false"
                    });
                }

                return false;
            }

            return string.Equals(configuration.Trim(), "true", StringComparison.OrdinalIgnoreCase);
        }
    }

    internal static class VendorPrice
    {
        public static async Task Apply(IRepository repository, int productVendorId, int productId, int unit, decimal newCost, string source)
        {
            var vendorProduct = await repository.Query<domain.Data.ProductVendor>()
                .FirstOrDefaultAsync(item => item.Id == productVendorId);
            if (vendorProduct == null || newCost <= 0)
                return;

            var units = unit <= 0 ? 1 : unit;
            vendorProduct.Cost = newCost;
            vendorProduct.UnitCost = newCost / units;
            repository.UpdateSync(vendorProduct);

            var product = await repository.Query<domain.Data.Product>().FirstOrDefaultAsync(item => item.Id == productId);
            if (product != null)
            {
                product.Cost = vendorProduct.UnitCost;
                repository.UpdateSync(product);
            }

            repository.InsertAsync(new domain.General.DataChangeLog
            {
                Date = Clock.Now,
                Entity = "PRODUCT",
                EntityId = productId,
                Log = source + ". Costo nuevo: " + newCost
            });
        }
    }
}
