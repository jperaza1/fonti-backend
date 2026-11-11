using cod.application.DispatchControl.Commom;
using cod.application.DispatchControl.Services;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.domain.Features;
using cod.domain.Features.Enums;
using cod.domain.Operation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.DispatchControl.Commands.CreateDispatchTransfer
{
    public class CreateDispatchTransferCommand : BaseCommand, ICreateDispatchTransferCommand
    {
        private readonly AuthService _authService;
        private readonly IDispatchTransferSettings _transferSettings;

        public CreateDispatchTransferCommand(
            IRepository repository,
            IExceptionFactory exceptionFactory,
            AuthService authService,
            IDispatchTransferSettings transferSettings) : base(repository, exceptionFactory)
        {
            _authService = authService;
            _transferSettings = transferSettings;
        }

        public async Task Execute(DispatchTransferModel model)
        {
            if (!await _transferSettings.IsEnabled())
            {
                this._exceptionFactory.Add("transfer", "El traspaso entre rutas está apagado.");
                throw new ApplicationException("CANCELED", "Control de Salida", this._exceptionFactory.Exceptions);
            }

            if (model == null)
                model = new DispatchTransferModel();

            if (model.FromDispatchControlId <= 0)
                this._exceptionFactory.Add("dispatch", "El control de salida no existe.");

            if (model.ToRouteId <= 0)
                this._exceptionFactory.Add("toRouteId", "Elige la ruta que recibe.");

            if (model.ProductId <= 0)
                this._exceptionFactory.Add("productId", "Elige un producto de venta.");

            if (model.Quantity <= 0)
                this._exceptionFactory.Add("quantity", "La cantidad tiene que ser mayor a 0.");

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "Control de Salida", this._exceptionFactory.Exceptions);

            var source = await _repository.QueryActive<domain.Operation.DispatchControl>()
                .Where(control => control.Id == model.FromDispatchControlId)
                .Select(control => new
                {
                    control.Id,
                    control.RouteId,
                    RouteName = control.Route.Name,
                    control.Status,
                    control.DispatchControlDate
                })
                .FirstOrDefaultAsync();

            if (source == null)
            {
                this._exceptionFactory.Add("dispatch", "El control de salida no existe.");
                throw new ApplicationException("CANCELED", "Control de Salida", this._exceptionFactory.Exceptions);
            }

            if (source.Status != DispatchControlStatus.Open)
                this._exceptionFactory.Add("dispatch", "El control de salida ya está cerrado.");

            if (model.ToRouteId == source.RouteId)
                this._exceptionFactory.Add("toRouteId", "Elige otra ruta.");

            var product = await _repository.QueryActive<domain.Data.Product>()
                .Where(item => item.Id == model.ProductId && item.ProductType == ProductType.Sale)
                .Select(item => new { item.Id, item.Name })
                .FirstOrDefaultAsync();

            if (product == null)
                this._exceptionFactory.Add("productId", "Elige un producto de venta.");

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "Control de Salida", this._exceptionFactory.Exceptions);

            var destination = await _repository.QueryActive<domain.Operation.DispatchControl>()
                .Where(control => control.RouteId == model.ToRouteId
                    && control.Id != source.Id
                    && control.Status == DispatchControlStatus.Open
                    && control.DispatchControlDate.Date == source.DispatchControlDate.Date)
                .OrderByDescending(control => control.Id)
                .Select(control => new
                {
                    control.Id,
                    control.RouteId,
                    RouteName = control.Route.Name
                })
                .FirstOrDefaultAsync();

            if (destination == null)
            {
                this._exceptionFactory.Add("toRouteId", "La ruta destino no tiene un control de salida abierto en esa fecha.");
                throw new ApplicationException("CANCELED", "Control de Salida", this._exceptionFactory.Exceptions);
            }

            var available = await Available(source.Id, product.Id);
            if (model.Quantity > available)
            {
                this._exceptionFactory.Add(
                    "quantity",
                    "No hay suficiente de " + product.Name + " en la ruta. Disponible: " + available + ".");
                throw new ApplicationException("CANCELED", "Control de Salida", this._exceptionFactory.Exceptions);
            }

            var transfer = new DispatchTransfer
            {
                FromDispatchControlId = source.Id,
                ToDispatchControlId = destination.Id,
                ProductId = product.Id,
                Quantity = model.Quantity
            };
            _repository.InsertAsync(transfer);

            _repository.InsertAsync(CreateDetail(
                source.Id,
                DispatchControlType.TransferOut,
                destination.RouteId,
                transfer,
                product.Id,
                model.Quantity));
            _repository.InsertAsync(CreateDetail(
                destination.Id,
                DispatchControlType.TransferIn,
                source.RouteId,
                transfer,
                product.Id,
                model.Quantity));

            await _repository.CommitAsync();
        }

        private DispatchControlDetail CreateDetail(
            int dispatchControlId,
            DispatchControlType type,
            int counterpartRouteId,
            DispatchTransfer transfer,
            int productId,
            int quantity)
        {
            var detail = new DispatchControlDetail
            {
                DispatchControlId = dispatchControlId,
                DispatchControlType = type,
                BottleQuantity = 0,
                CrateQuantity = 0,
                MissingBottleQuantity = 0,
                CounterpartRouteId = counterpartRouteId,
                DispatchTransfer = transfer,
                IsActive = true,
                CreationTime = Clock.Now,
                CreatorUser = _authService.User
            };

            detail.ProductMovements = new List<ProductMovement>
            {
                new ProductMovement
                {
                    ProductId = productId,
                    Quantity = quantity,
                    IsActive = true,
                    CreationTime = Clock.Now,
                    CreatorUser = _authService.User
                }
            };

            return detail;
        }

        private async Task<int> Available(int dispatchControlId, int productId)
        {
            var details = await _repository.QueryActive<DispatchControlDetail>()
                .Where(detail => detail.DispatchControlId == dispatchControlId)
                .Select(detail => new
                {
                    detail.DispatchControlType,
                    Quantity = detail.ProductMovements
                        .Where(movement => movement.IsActive && movement.ProductId == productId)
                        .Sum(movement => (int?)movement.Quantity) ?? 0
                })
                .ToListAsync();

            var available = 0;
            foreach (var detail in details)
                available += DispatchQuantity.Sign(detail.DispatchControlType) * detail.Quantity;

            return available;
        }
    }
}
