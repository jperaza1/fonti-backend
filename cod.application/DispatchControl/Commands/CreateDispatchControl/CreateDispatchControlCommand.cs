using cod.application.DispatchControl.Commom;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Inventory.Services;
using cod.domain.Features;
using cod.domain.Operation;
using DocuSign.eSign.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.DispatchControl.Commands.CreateDispatchControl
{
    public class CreateDispatchControlCommand : BaseCommand, ICreateDispatchControlCommand
    {
        private readonly AuthService _authService;
        private readonly IInventoryMovementService _inventoryMovementService;
        public CreateDispatchControlCommand(
            IRepository repository, 
            IExceptionFactory exceptionFactory,
            AuthService authService,
            IInventoryMovementService inventoryMovementService) : base(repository, exceptionFactory)
        {
            _authService = authService;
            _inventoryMovementService = inventoryMovementService;
        }

        public async Task Execute(DispatchControlModel model)
        {
            await ValidateModel(model);

            var dispatchControl = new domain.Operation.DispatchControl();
            dispatchControl.DispatchControlDate = model.DispatchControlDate;
            dispatchControl.Vendor = model.Vendor;
            dispatchControl.RouteId = model.RouteId;
            dispatchControl.Status = domain.Features.Enums.DomainEnums.DispatchControlStatus.Open;
            dispatchControl.DispatchControlDetails = CreateDetails(model, _authService);
            dispatchControl.TruckId = model.TruckId;

            _repository.InsertAsync(dispatchControl);
            await _inventoryMovementService.ApplyDispatch(dispatchControl);

            await _repository.CommitAsync();        }

        public List<DispatchControlDetail> CreateDetails(DispatchControlModel model, AuthService authService)
        {
            var lst = new List<DispatchControlDetail>();

            foreach(var item in model.DispatchControlDetails)
            {
                if (DispatchQuantity.IsTransfer(item.DispatchControlType))
                    continue;

                var detail = new DispatchControlDetail();
                detail.DispatchControlType = item.DispatchControlType;
                detail.MissingBottleQuantity = item.MissingBottleQuantity;
                detail.BottleQuantity = item.BottleQuantity;
                detail.MissingBottleQuantity = item.MissingBottleQuantity;
                detail.CrateQuantity = item.CrateQuantity;
                detail.IsActive = true;
                detail.CreationTime = Clock.Now;
                detail.CreatorUser = authService.User;
                detail.ProductMovements = CreateProductMovement(item, authService);

                lst.Add(detail);
            }

            return lst;
        }

        public List<ProductMovement> CreateProductMovement(DispatchControlDetailModel model, AuthService authService)
        {
            var lst = new List<ProductMovement>();

            foreach(var item in model.ProductMovements)
            {
                var product = new ProductMovement();
                product.ProductId = item.ProductId;
                product.Quantity = item.Quantity;
                product.IsActive = true;
                product.CreatorUser = authService.User;
                product.CreationTime = Clock.Now;

                lst.Add(product);
            }

            return lst;
        }

        public async Task ValidateModel(DispatchControlModel model) 
        {
            if (model.RouteId <= 0)
                this._exceptionFactory.Add("route", this._exceptionFactory.GetMessage("Ruta", ExceptionEnums.ExceptionCode.Required));

            if(model.DispatchControlDate == DateTime.MinValue)
                this._exceptionFactory.Add("dispatchControlDate", this._exceptionFactory.GetMessage("Fecha", ExceptionEnums.ExceptionCode.Required));

            if(string.IsNullOrWhiteSpace(model.Vendor))
                this._exceptionFactory.Add("vendor", this._exceptionFactory.GetMessage("Vendedor", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "Control de Salida", this._exceptionFactory.Exceptions);

            foreach(var item in model.DispatchControlDetails)
            {
                if (DispatchQuantity.IsTransfer(item.DispatchControlType))
                    continue;

                if(item.DispatchControlType != domain.Features.Enums.DomainEnums.DispatchControlType.Return)
                {
                    if(item.CrateQuantity <= 0)
                        this._exceptionFactory.Add("crateQuantity", this._exceptionFactory.GetMessage("Cestas", ExceptionEnums.ExceptionCode.Required));

                    if(item.BottleQuantity <= 0)
                        this._exceptionFactory.Add("bottleQuantity", this._exceptionFactory.GetMessage("Botes", ExceptionEnums.ExceptionCode.Required));
                }

                if(item.DispatchControlType == domain.Features.Enums.DomainEnums.DispatchControlType.Return)
                {
                    if (item.MissingBottleQuantity < 0)
                        this._exceptionFactory.Add("missingBottleQuantity", this._exceptionFactory.GetMessage("Los botes faltantes no pueden ser negativos", ExceptionEnums.ExceptionCode.Default));
                }

            }

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "Control de Salida", this._exceptionFactory.Exceptions);

        }

    }
}
