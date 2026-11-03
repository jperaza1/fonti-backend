using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.PurchaseOrder.Common;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.PurchaseOrder.Commands.UpdatePurchaseOrder
{
    public class UpdatePurchaseOrderCommand : BaseCommand, IUpdatePurchaseOrderCommand
    {
        public UpdatePurchaseOrderCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(PurchaseOrderModel model, int id)
        {
            if (string.IsNullOrEmpty(model.TypeInoice))
                _exceptionFactory.Add("typeInoice", _exceptionFactory.GetMessage("Tipo de Factura", ExceptionEnums.ExceptionCode.Required));

            if (model.Details == null || !model.Details.Any())
                _exceptionFactory.Add("details", _exceptionFactory.GetMessage("Producto", ExceptionEnums.ExceptionCode.Required));

            var invalidReceived = model.Details != null && model.Details.Any(item =>
                (item.DetailStatus == DetailStatus.Prince_Quantity_Mismatch || item.DetailStatus == DetailStatus.Price_Changed)
                && (item.QuantityRecived <= 0 || item.CostRecived <= 0));

            if (invalidReceived)
                _exceptionFactory.Add("details", "Se debe ingresar la cantidad recibida y el costo recibido");

            if (!_exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "ORDEN COMPRA", _exceptionFactory.Exceptions);

            var purchaseOrder = await _repository.Query<domain.Operation.PurchaseOrder>().FirstOrDefaultAsync(item => item.Id == id);
            if (purchaseOrder == null)
                throw new ApplicationException("Orden de compra no existe", "ORDEN COMPRA");

            if (purchaseOrder.OrderStatus != PurchaseOrderStatus.Created && purchaseOrder.OrderStatus != PurchaseOrderStatus.Processing)
                throw new ApplicationException("La orden no se puede editar en este estado", "ORDEN COMPRA");

            purchaseOrder.TypeInoice = model.TypeInoice;
            purchaseOrder.Total = Math.Round(model.Details.Sum(item => item.Total), 2);
            _repository.UpdateSync(purchaseOrder);

            var details = await _repository.WhereAsync<domain.Operation.PurchaseOrderDetail>(item => item.PurchaseOrderId == id && item.IsActive).ToListAsync();
            var removed = details.Where(detail => model.Details.All(item => item.Id != detail.Id)).ToList();
            foreach (var item in removed)
                _repository.DeleteSync(item);

            foreach (var item in model.Details.Where(detail => detail.Id <= 0))
            {
                var line = Build(item);
                line.PurchaseOrderId = id;
                line.DetailStatus = item.DetailStatus.IsValid() ? item.DetailStatus : DetailStatus.Pending;
                _repository.InsertAsync(line);
            }

            foreach (var item in model.Details.Where(detail => detail.Id > 0))
            {
                var amounts = Amounts(item.Cost, item.Quantity, item.DiscountPercentage, item.TaxPercentage);
                var received = Amounts(item.CostRecived, item.QuantityRecived, item.DiscountPercentage, item.TaxPercentage);
                await _repository.UpdateAsync<domain.Operation.PurchaseOrderDetail>(
                    detail => detail.Id == item.Id,
                    detail => new domain.Operation.PurchaseOrderDetail
                    {
                        Quantity = item.Quantity,
                        Cost = item.Cost,
                        SubTotal = amounts.SubTotal,
                        Discount = amounts.Discount,
                        DiscountPercentage = item.DiscountPercentage,
                        Tax = amounts.Tax,
                        TaxPercentage = item.TaxPercentage,
                        Total = amounts.Total,
                        DetailStatus = item.DetailStatus,
                        QuantityRecived = item.QuantityRecived,
                        CostRecived = item.CostRecived,
                        SubTotalRecived = received.SubTotal,
                        DiscountRecived = received.Discount,
                        TaxRecived = received.Tax,
                        TotalRecived = received.Total
                    });
            }

            await _repository.CommitAsync();
        }

        private static domain.Operation.PurchaseOrderDetail Build(PurchaseOrderDetailModel item)
        {
            var amounts = Amounts(item.Cost, item.Quantity, item.DiscountPercentage, item.TaxPercentage);
            return new domain.Operation.PurchaseOrderDetail
            {
                ProductId = item.ProductId,
                ProductVendorId = item.ProductVendorId,
                Quantity = item.Quantity,
                Cost = item.Cost,
                SubTotal = amounts.SubTotal,
                Discount = amounts.Discount,
                DiscountPercentage = item.DiscountPercentage,
                Tax = amounts.Tax,
                TaxPercentage = item.TaxPercentage,
                Total = amounts.Total,
                DetailStatus = DetailStatus.Pending
            };
        }

        private static (double SubTotal, double Discount, double Tax, double Total) Amounts(double cost, double quantity, double discountPercentage, double taxPercentage)
        {
            var subTotal = Math.Round(cost * quantity, 2, MidpointRounding.AwayFromZero);
            var discount = Math.Round(discountPercentage >= 1 ? quantity * discountPercentage : subTotal * discountPercentage, 2, MidpointRounding.AwayFromZero);
            var tax = Math.Round((subTotal - discount) * taxPercentage, 2, MidpointRounding.AwayFromZero);
            var total = Math.Round(subTotal + tax - discount, 2, MidpointRounding.AwayFromZero);
            return (subTotal, discount, tax, total);
        }
    }
}
