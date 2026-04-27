using cod.application.Common;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.PurchaseOrder.Common;
using cod.domain.Common;
using cod.domain.Data;
using cod.domain.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.PurchaseOrder.Commands.UpdatePurchaseOrder
{
    public class UpdatePurchaseOrderCommand : BaseCommand, IUpdatePurchaseOrderCommand
    {
        private readonly ILogHelper _logHelper;

        public UpdatePurchaseOrderCommand(
            IRepository repository, 
            IExceptionFactory exceptionFactory,
            ILogHelper logHelper) : base(repository, exceptionFactory)
        {
            this._logHelper = logHelper;
        }

        public async Task Execute(PurchaseOrderModel model, int id)
        {
            ValidateModel(model);
            _logHelper.Initialize(_repository);
            List<CompareModel> logs = new List<CompareModel>();

            var purchaseOrder = await _repository.Query<domain.Operation.PurchaseOrder>().FirstOrDefaultAsync(f => f.Id == id);

            if (purchaseOrder == null)
                throw new ApplicationException("Orden de compra no existe", "ORDEN COMPRA");

            purchaseOrder.TypeInoice = model.TypeInoice;
            purchaseOrder.Total = Math.Round(model.Details.Sum(f => f.Total), 2);

            _repository.UpdateSync(purchaseOrder);

            var details = await _repository.AllActive<domain.Operation.PurchaseOrderDetail>().Where(f => f.PurchaseOrderId == id).ToListAsync();

            var detailsToRemove = details.Where(detail => model.Details.All(member => member.Id != detail.Id)).ToList();

            foreach (var item in detailsToRemove)
            {
                _repository.InsertAsync(new domain.General.DataChangeLog
                {
                    Date = Clock.Now,
                    Entity = "PURCHASE_ORDER",
                    EntityId = id,
                    Log = $"(Elimando Producto) => {item.Product.Name} - Cantidad: {item.Quantity}"
                });
                _repository.DeleteSync(item);
            }

            var detailToAdd = model.Details.Where(dd => dd.Id <= 0).ToList();

            foreach (var item in detailToAdd)
            {
                _repository.InsertAsync(new domain.General.DataChangeLog
                {
                    Date = Clock.Now,
                    Entity = "PURCHASE_ORDER",
                    EntityId = id,
                    Log = $"(Agregando Producto) => {item.Product} - Cantidad: {item.Quantity}"
                });
                _repository.InsertAsync(new domain.Operation.PurchaseOrderDetail
                {
                    PurchaseOrderId = id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    SubTotal = Math.Round(item.SubTotal, 2),
                    Tax = Math.Round(item.Tax, 2),
                    TaxPercentage = item.TaxPercentage,
                    Discount = Math.Round(item.Discount, 2),
                    DiscountPercentage = item.DiscountPercentage,
                    Cost = item.Cost,
                    Total = Math.Round(item.Total, 2),
                    DetailStatus = item.DetailStatus.IsValid() ? item.DetailStatus : DetailStatus.Received,
                    QuantityRecived = item.DetailStatus.IsValid() ? item.QuantityRecived : item.Quantity,
                    CostRecived = item.DetailStatus.IsValid() ? item.CostRecived : item.Cost,
                    TotalRecived = item.DetailStatus.IsValid() ? Math.Round(item.TotalRecived, 2) : Math.Round(item.Total, 2),
                    SubTotalRecived = item.DetailStatus.IsValid() ? Math.Round(item.SubTotalRecived, 2) : Math.Round(item.SubTotal, 2),
                    TaxRecived = item.DetailStatus.IsValid() ? Math.Round(item.TaxRecived, 2) : Math.Round(item.Tax, 2),
                    DiscountRecived = item.DetailStatus.IsValid() ? Math.Round(item.DiscountRecived, 2) : Math.Round(item.Discount, 2),
                });
            }

            var detailToUpdate = model.Details.Where(dd => dd.Id > 0).ToList();

            foreach (var item in detailToUpdate)
            {
                var detail = await _repository.Query<domain.Operation.PurchaseOrderDetail>()
                    .Where(f => f.Id == item.Id).AsNoTracking().FirstOrDefaultAsync();

                _logHelper.CompareField(detail.Quantity, item.Quantity, logs, "Cantidad");
                _logHelper.CompareField(detail.QuantityRecived, item.QuantityRecived, logs, "Cantidad Recibida");
                _logHelper.CompareField(detail.CostRecived, item.CostRecived, logs, "Costo Recibido");
                _logHelper.CompareField(detail.DetailStatus, item.DetailStatus, logs, "Estado");

                await _repository.UpdateAsync<domain.Operation.PurchaseOrderDetail>(
                type => type.Id == item.Id,
                type => new domain.Operation.PurchaseOrderDetail
                {
                    Quantity = item.Quantity,
                    SubTotal = Math.Round(item.SubTotal, 2),
                    Tax = Math.Round(item.Tax, 2),
                    TaxPercentage = item.TaxPercentage,
                    Discount = Math.Round(item.Discount, 2),
                    DiscountPercentage = item.DiscountPercentage,
                    Total = Math.Round(item.Total, 2),
                    DetailStatus = item.DetailStatus,
                    QuantityRecived = item.QuantityRecived,
                    CostRecived = item.CostRecived,
                    TotalRecived = Math.Round(item.TotalRecived, 2),
                    SubTotalRecived = Math.Round(item.SubTotalRecived, 2),
                    TaxRecived = Math.Round(item.TaxRecived, 2),
                    DiscountRecived = Math.Round(item.DiscountRecived, 2)
                });
            }

            _logHelper.InsertLogs(logs, "PURCHASE_ORDER", id);

            await _repository.CommitAsync();

        }

        public void ValidateModel(PurchaseOrderModel model)
        {
            if (string.IsNullOrEmpty(model.TypeInoice))
                this._exceptionFactory.Add("typeInoice", this._exceptionFactory.GetMessage("Tipo de Factura", ExceptionEnums.ExceptionCode.Required));

            var priceQuantityMismatchError = model.Details.Any(f => f.DetailStatus == DetailStatus.Prince_Quantity_Mismatch && (f.QuantityRecived <= 0 || f.CostRecived <= 0));

            if(priceQuantityMismatchError)
                this._exceptionFactory.Add("details", this._exceptionFactory.GetMessage("Se debe de ingresar la cantidad recibida o el costo recibido", ExceptionEnums.ExceptionCode.Required));

                if (!this._exceptionFactory.IsValid())
                    throw new ApplicationException("CANCELED", "CAFETERIA", this._exceptionFactory.Exceptions);
        }
    }
}
