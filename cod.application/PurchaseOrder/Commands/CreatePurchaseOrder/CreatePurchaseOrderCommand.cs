using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.PurchaseOrder.Common;
using cod.domain.Features.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.PurchaseOrder.Commands.CreatePurchaseOrder
{
    public class CreatePurchaseOrderCommand : BaseCommand, ICreatePurchaseOrderCommand
    {
        public CreatePurchaseOrderCommand(
            IRepository repository,
            IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task<int> Execute(PurchaseOrderModel model)
        {
            var lines = await ValidateModel(model);
            var count = await _repository.Query<domain.Operation.PurchaseOrder>().CountAsync();
            var reference = "OC-" + (count + 1).ToString("D5");

            var purchaseOrder = new domain.Operation.PurchaseOrder
            {
                VendorId = model.VendorId,
                Date = model.Date,
                Reference = reference,
                Total = lines.Sum(item => item.Total),
                OrderStatus = PurchaseOrderStatus.Created,
                TypeInoice = model.TypeInoice,
                PurchaseOrderDetails = lines
            };

            foreach (var line in lines)
                _repository.SetAuthValuesToCreate(line);

            _repository.InsertAsync(purchaseOrder);
            await _repository.CommitAsync();
            return purchaseOrder.Id;
        }

        private async Task<List<domain.Operation.PurchaseOrderDetail>> ValidateModel(PurchaseOrderModel model)
        {
            if (model.VendorId <= 0)
                _exceptionFactory.Add("vendor", _exceptionFactory.GetMessage("Proveedor", ExceptionEnums.ExceptionCode.Required));

            if (string.IsNullOrEmpty(model.TypeInoice))
                _exceptionFactory.Add("typeInoice", _exceptionFactory.GetMessage("Tipo de Factura", ExceptionEnums.ExceptionCode.Required));

            if (model.Date == DateTime.MinValue)
                _exceptionFactory.Add("date", _exceptionFactory.GetMessage("Fecha", ExceptionEnums.ExceptionCode.Required));

            if (model.Details == null || model.Details.Count == 0)
                _exceptionFactory.Add("details", _exceptionFactory.GetMessage("Producto", ExceptionEnums.ExceptionCode.Required));

            if (!_exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "ORDEN COMPRA", _exceptionFactory.Exceptions);

            var vendorExists = await _repository.AnyActive<domain.Data.Vendor>(vendor => vendor.Id == model.VendorId);
            if (!vendorExists)
                _exceptionFactory.Add("vendor", "El proveedor no existe");

            var ids = model.Details.Select(item => item.ProductVendorId ?? 0).Where(id => id > 0).Distinct().ToList();
            var products = await _repository.QueryActive<domain.Data.ProductVendor>()
                .Where(item => ids.Contains(item.Id))
                .Select(item => new
                {
                    item.Id,
                    item.VendorId,
                    item.ProductId,
                    item.Cost,
                    item.Discount,
                    item.Unit,
                    item.Product.Tax,
                    item.Product.ProductType,
                    item.Product.Name
                })
                .ToListAsync();

            var lines = new List<domain.Operation.PurchaseOrderDetail>();
            foreach (var item in model.Details)
            {
                if (item.Quantity <= 0)
                    _exceptionFactory.Add("details", "La cantidad debe ser mayor a cero");

                var product = products.FirstOrDefault(current => current.Id == item.ProductVendorId);
                if (product == null || product.VendorId != model.VendorId)
                    _exceptionFactory.Add("details", "El producto no pertenece al proveedor");
                else if (product.ProductType != ProductType.RawMaterial)
                    _exceptionFactory.Add("details", "La orden de compra solo admite materia prima");
                else
                    lines.Add(BuildDetail(item, product.ProductId, product.Cost, product.Discount, product.Tax));
            }

            if (!_exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "ORDEN COMPRA", _exceptionFactory.Exceptions);

            return lines;
        }

        private static domain.Operation.PurchaseOrderDetail BuildDetail(
            PurchaseOrderDetailModel item,
            int productId,
            decimal cost,
            decimal discountRate,
            bool tax)
        {
            var unitCost = (double)cost;
            var discountPercentage = (double)discountRate;
            var taxPercentage = tax ? 0.15d : 0d;
            var subTotal = Math.Round(unitCost * item.Quantity, 2, MidpointRounding.AwayFromZero);
            var discount = Math.Round(discountPercentage >= 1 ? item.Quantity * discountPercentage : subTotal * discountPercentage, 2, MidpointRounding.AwayFromZero);
            var taxAmount = Math.Round((subTotal - discount) * taxPercentage, 2, MidpointRounding.AwayFromZero);
            var total = Math.Round(subTotal + taxAmount - discount, 2, MidpointRounding.AwayFromZero);

            return new domain.Operation.PurchaseOrderDetail
            {
                ProductId = productId,
                ProductVendorId = item.ProductVendorId,
                Quantity = item.Quantity,
                Cost = unitCost,
                SubTotal = subTotal,
                Discount = discount,
                DiscountPercentage = discountPercentage,
                Tax = taxAmount,
                TaxPercentage = taxPercentage,
                Total = total,
                DetailStatus = DetailStatus.Pending
            };
        }
    }
}
