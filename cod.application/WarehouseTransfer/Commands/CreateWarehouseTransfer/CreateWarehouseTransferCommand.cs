using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Inventory.Services;
using cod.application.WarehouseTransfer.Common;
using cod.domain.Features;
using cod.domain.Features.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.WarehouseTransfer.Commands.CreateWarehouseTransfer
{
    public class CreateWarehouseTransferCommand : BaseCommand, ICreateWarehouseTransferCommand
    {
        private readonly AuthService _authService;
        private readonly IInventoryMovementService _inventory;
        private readonly IWarehouseDirectory _warehouses;

        public CreateWarehouseTransferCommand(
            IRepository repository,
            IExceptionFactory exceptionFactory,
            AuthService authService,
            IInventoryMovementService inventory,
            IWarehouseDirectory warehouses) : base(repository, exceptionFactory)
        {
            _authService = authService;
            _inventory = inventory;
            _warehouses = warehouses;
        }

        public async Task<int> Execute(WarehouseTransferModel model)
        {
            var origin = await _warehouses.Get(model == null ? 0 : model.FromWarehouseId);
            var destination = await _warehouses.Get(model == null ? 0 : model.ToWarehouseId);
            var lines = await ValidLines(model, origin, destination);

            var transfer = new domain.Operation.WarehouseTransfer
            {
                Date = model.Date,
                FromWarehouseId = origin.Id,
                ToWarehouseId = destination.Id,
                Note = string.IsNullOrWhiteSpace(model.Note) ? null : model.Note.Trim(),
                Status = WarehouseTransferStatus.Pending,
                SentUser = _authService.User,
                Details = lines.Select(line => new domain.Operation.WarehouseTransferDetail
                {
                    ProductId = line.ProductId,
                    Quantity = line.Quantity
                }).ToList()
            };

            foreach (var detail in transfer.Details)
                _repository.SetAuthValuesToCreate(detail);

            _repository.InsertAsync(transfer);

            foreach (var line in lines)
            {
                var note = "Transferencia a " + destination.Name + " (" + line.Name + ")";
                await _inventory.ApplyTransferOut(
                    origin.Id,
                    line.ProductId,
                    line.Quantity,
                    model.Date,
                    note,
                    line.Cost);
            }

            await _repository.CommitAsync();
            return transfer.Id;
        }

        private async Task<List<Line>> ValidLines(
            WarehouseTransferModel model,
            domain.Operation.Warehouse origin,
            domain.Operation.Warehouse destination)
        {
            if (model == null)
                _exceptionFactory.Add("transfer", "La transferencia es obligatoria");

            if (model == null || model.FromWarehouseId <= 0)
                _exceptionFactory.Add("fromWarehouseId", _exceptionFactory.GetMessage("Bodega origen", ExceptionEnums.ExceptionCode.Required));

            if (model == null || model.ToWarehouseId <= 0)
                _exceptionFactory.Add("toWarehouseId", _exceptionFactory.GetMessage("Bodega destino", ExceptionEnums.ExceptionCode.Required));

            if (model != null && model.FromWarehouseId > 0 && model.FromWarehouseId == model.ToWarehouseId)
                _exceptionFactory.Add("toWarehouseId", "El origen y el destino tienen que ser bodegas distintas");

            if (model == null || model.Date == System.DateTime.MinValue)
                _exceptionFactory.Add("date", _exceptionFactory.GetMessage("Fecha", ExceptionEnums.ExceptionCode.Required));

            if (origin == null)
                _exceptionFactory.Add("fromWarehouseId", "La bodega origen no existe");

            if (destination == null)
                _exceptionFactory.Add("toWarehouseId", "La bodega destino no existe");

            var source = model?.Details ?? new List<WarehouseTransferDetailModel>();
            var grouped = source
                .Where(line => line != null && line.ProductId > 0 && line.Quantity > 0)
                .GroupBy(line => line.ProductId)
                .Select(group => new Line
                {
                    ProductId = group.Key,
                    Quantity = group.Sum(line => line.Quantity)
                })
                .ToList();

            if (grouped.Count == 0)
                _exceptionFactory.Add("details", "Agrega al menos un producto con cantidad mayor a cero");

            if (!_exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "TRANSFERENCIA", _exceptionFactory.Exceptions);

            var ids = grouped.Select(line => line.ProductId).ToList();
            var products = await _repository.QueryActive<domain.Data.Product>()
                .Where(product => ids.Contains(product.Id))
                .Select(product => new { product.Id, product.Name, product.ProductType, product.Cost })
                .ToListAsync();

            foreach (var line in grouped)
            {
                var product = products.FirstOrDefault(item => item.Id == line.ProductId);
                if (product == null)
                {
                    _exceptionFactory.Add("productId", "El producto no existe");
                    continue;
                }

                if (!_warehouses.Allows(destination.Kind, product.ProductType))
                {
                    _exceptionFactory.Add("productId", destination.Name + " no admite " + product.Name);
                    continue;
                }

                line.Name = product.Name;
                line.Cost = product.Cost;
            }

            if (!_exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "TRANSFERENCIA", _exceptionFactory.Exceptions);

            return grouped;
        }

        private class Line
        {
            public int ProductId { get; set; }
            public decimal Quantity { get; set; }
            public string Name { get; set; }
            public decimal? Cost { get; set; }
        }
    }
}
