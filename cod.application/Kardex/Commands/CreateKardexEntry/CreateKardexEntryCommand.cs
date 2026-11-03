using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Inventory.Services;
using cod.application.Kardex.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Kardex.Commands.CreateKardexEntry
{
    public class CreateKardexEntryCommand : BaseCommand, ICreateKardexEntryCommand
    {
        private readonly IInventoryMovementService _inventoryMovementService;

        public CreateKardexEntryCommand(
            IRepository repository,
            IExceptionFactory exceptionFactory,
            IInventoryMovementService inventoryMovementService) : base(repository, exceptionFactory)
        {
            _inventoryMovementService = inventoryMovementService;
        }

        public async Task Execute(KardexEntryModel model)
        {
            var product = await ValidateModel(model);

            var isQualityExit = model.IsExit && product.ProductType == domain.Features.Enums.ProductType.RawMaterial;
            var note = string.IsNullOrWhiteSpace(model.Note)
                ? (isQualityExit ? "Salida por mala calidad de " : model.IsExit ? "Salida de " : "Entrada de ") + product.Name
                : model.Note.Trim();

            if (isQualityExit)
            {
                await _inventoryMovementService.ApplyQualityExit(
                    model.ProductId,
                    model.Quantity,
                    model.MovementDate,
                    note,
                    product.Cost);
            }
            else if (model.IsExit)
            {
                await _inventoryMovementService.ApplyExit(
                    model.ProductId,
                    model.Quantity,
                    model.MovementDate,
                    note,
                    product.Cost);
            }
            else
            {
                await _inventoryMovementService.ApplyEntry(
                    model.ProductId,
                    model.Quantity,
                    model.MovementDate,
                    note,
                    product.Cost);
            }

            await _repository.CommitAsync();
        }

        private async Task<domain.Data.Product> ValidateModel(KardexEntryModel model)
        {
            if (model.ProductId <= 0)
                _exceptionFactory.Add("productId", _exceptionFactory.GetMessage("Producto", ExceptionEnums.ExceptionCode.Required));

            if (model.Quantity <= 0)
                _exceptionFactory.Add("quantity", "La cantidad debe ser mayor a cero");

            if (model.MovementDate == DateTime.MinValue)
                _exceptionFactory.Add("movementDate", _exceptionFactory.GetMessage("Fecha", ExceptionEnums.ExceptionCode.Required));

            if (!_exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "KARDEX", _exceptionFactory.Exceptions);

            var product = await _repository.QueryActive<domain.Data.Product>()
                .Where(f => f.Id == model.ProductId)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                _exceptionFactory.Add("productId", "El producto no existe");
                throw new ApplicationException("CANCELED", "KARDEX", _exceptionFactory.Exceptions);
            }

            return product;
        }
    }
}
