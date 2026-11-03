using cod.application.DispatchControl.Commom;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Inventory.Services;
using cod.domain.Features;
using cod.domain.Operation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iTextSharp.text.pdf.AcroFields;
using ApplicationException = cod.application.Features.Exception.ApplicationException;


namespace cod.application.DispatchControl.Commands.UpdateDispatchControl
{
    public class UpdateDispatchControlCommand : BaseCommand, IUpdateDispatchControlCommand
    {
        private readonly AuthService _authService;
        private readonly IInventoryMovementService _inventoryMovementService;
        public UpdateDispatchControlCommand(
            IRepository repository, 
            IExceptionFactory exceptionFactory,
            AuthService authService,
            IInventoryMovementService inventoryMovementService) : base(repository, exceptionFactory)
        {
            _authService = authService;
            _inventoryMovementService = inventoryMovementService;
        }

        public async Task Execute(DispatchControlModel model, int id)
        {
            await ValidateModel(model);

            var dispatch = await _repository.Query<domain.Operation.DispatchControl>()
                .Where(f => f.Id == id)
                .FirstOrDefaultAsync();

            if(dispatch == null)
            {
                throw new ApplicationException("CANCELED", "Control de Salida", this._exceptionFactory.Exceptions);
            }

            var addDetail = model.DispatchControlDetails.Where(f => f.Id <= 0).ToList();
            var updateDetail = model.DispatchControlDetails.Where(f => f.Id > 0).ToList();

            foreach(var item in addDetail)
            {
                var detail = new DispatchControlDetail();
                detail.DispatchControlId = id;
                detail.DispatchControlType = item.DispatchControlType;
                detail.MissingBottleQuantity = item.MissingBottleQuantity;
                detail.BottleQuantity = item.BottleQuantity;
                detail.MissingBottleQuantity = item.MissingBottleQuantity;
                detail.CrateQuantity = item.CrateQuantity;
                detail.ProductMovements = CreateProductMovement(item, _authService);

                if(item.DispatchControlType == domain.Features.Enums.DomainEnums.DispatchControlType.Return)
                {
                    dispatch.Status = domain.Features.Enums.DomainEnums.DispatchControlStatus.Close;
                    _repository.UpdateSync(dispatch);
                }

                _repository.InsertAsync(detail);
                await _inventoryMovementService.ApplyDetail(dispatch, detail);
            }

            foreach(var item in updateDetail)
            {
                var detail = await _repository.Query<domain.Operation.DispatchControlDetail>()
                    .Where(f => f.Id == item.Id)
                    .FirstOrDefaultAsync();

                detail.MissingBottleQuantity = item.MissingBottleQuantity;
                detail.BottleQuantity = item.BottleQuantity;
                detail.CrateQuantity = item.CrateQuantity;

                var productMovements = await _repository.Query<domain.Operation.ProductMovement>()
                    .Where(f => f.DispatchControlDetailId == item.Id)
                    .ToListAsync();

                foreach(var product in productMovements)
                {
                    var existProduct = productMovements.Where(f => f.Id == product.Id).FirstOrDefault();

                    existProduct.Quantity = product.Quantity;

                    _repository.UpdateSync(existProduct);
                }

                _repository.UpdateSync(detail);
            }

            await _repository.CommitAsync();
        }

        public List<ProductMovement> CreateProductMovement(DispatchControlDetailModel model, AuthService authService)
        {
            var lst = new List<ProductMovement>();

            foreach (var item in model.ProductMovements)
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

            if (model.DispatchControlDate == DateTime.MinValue)
                this._exceptionFactory.Add("dispatchControlDate", this._exceptionFactory.GetMessage("Fecha", ExceptionEnums.ExceptionCode.Required));

            if (string.IsNullOrWhiteSpace(model.Vendor))
                this._exceptionFactory.Add("vendor", this._exceptionFactory.GetMessage("Vendedor", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "Control de Salida", this._exceptionFactory.Exceptions);

            foreach (var item in model.DispatchControlDetails)
            {
                /*if (item.DispatchControlType == domain.Features.Enums.DomainEnums.DispatchControlType.Reload)
                {
                    if (item.CrateQuantity <= 0)
                        this._exceptionFactory.Add("crateQuantity", this._exceptionFactory.GetMessage("Cestas", ExceptionEnums.ExceptionCode.Required));

                    if (item.BottleQuantity <= 0)
                        this._exceptionFactory.Add("bottleQuantity", this._exceptionFactory.GetMessage("Botes", ExceptionEnums.ExceptionCode.Required));
                }

                if (item.DispatchControlType == domain.Features.Enums.DomainEnums.DispatchControlType.Return)
                {
                    
                }*/

            }

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "Control de Salida", this._exceptionFactory.Exceptions);

        }
    }
}
