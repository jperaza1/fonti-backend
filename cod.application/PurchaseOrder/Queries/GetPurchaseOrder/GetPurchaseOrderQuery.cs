using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.PurchaseOrder.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.PurchaseOrder.Queries.GetPurchaseOrder
{
    public class GetPurchaseOrderQuery : BaseQuery, IGetPurchaseOrderQuery
    {
        public GetPurchaseOrderQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<PurchaseOrderModel> Execute(int id)
        {
            var purchaseOrder = await _repository.Query<domain.Operation.PurchaseOrder>()
                .Where(item => item.Id == id)
                .Select(item => new
                {
                    item.Id,
                    item.VendorId,
                    Vendor = item.Vendor.Name,
                    item.OrderStatus,
                    item.Total,
                    item.Date,
                    item.Reference,
                    item.ReasonRejection,
                    item.ReasonCanceled,
                    item.CanceledAt,
                    item.CanceledUser,
                    item.TypeInoice,
                    item.CreatorUser,
                    item.ApprovedUser,
                    Details = item.PurchaseOrderDetails.Select(line => new
                    {
                        line.Id,
                        line.PurchaseOrderId,
                        line.ProductId,
                        line.ProductVendorId,
                        Product = line.ProductVendorId.HasValue
                            ? line.ProductVendor.Product.Name + " (" + line.ProductVendor.UnitOfMeasure.Name + ")"
                            : line.Product.Name,
                        line.Cost,
                        line.Quantity,
                        line.Discount,
                        line.DiscountPercentage,
                        line.Tax,
                        line.TaxPercentage,
                        line.SubTotal,
                        line.Total,
                        line.QuantityRecived,
                        line.CostRecived,
                        line.DetailStatus,
                        line.TotalRecived,
                        line.DiscountRecived,
                        line.TaxRecived,
                        line.SubTotalRecived
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (purchaseOrder == null)
                throw new ApplicationException("Orden no existe", "ORDEN COMPRA");

            return new PurchaseOrderModel
            {
                Id = purchaseOrder.Id,
                VendorId = purchaseOrder.VendorId,
                Vendor = purchaseOrder.Vendor,
                Total = purchaseOrder.Total,
                Date = purchaseOrder.Date.Date,
                Reference = purchaseOrder.Reference,
                OrderStatus = purchaseOrder.OrderStatus,
                Reason = purchaseOrder.ReasonRejection,
                ReasonCanceled = purchaseOrder.ReasonCanceled,
                CanceledUser = purchaseOrder.CanceledUser,
                CanceledAt = purchaseOrder.CanceledAt,
                OrderStatusName = purchaseOrder.OrderStatus.GetEnumDescription(),
                TypeInoice = purchaseOrder.TypeInoice,
                CreatorUser = purchaseOrder.CreatorUser,
                ApprovedUser = purchaseOrder.ApprovedUser,
                Details = purchaseOrder.Details.Select(line => new PurchaseOrderDetailModel
                {
                    Id = line.Id,
                    PurchaseOrderId = line.PurchaseOrderId,
                    Cost = line.Cost,
                    Quantity = line.Quantity,
                    Tax = line.Tax,
                    TaxPercentage = line.TaxPercentage,
                    SubTotal = line.SubTotal,
                    Total = line.Total,
                    Product = line.Product,
                    ProductId = line.ProductId,
                    ProductVendorId = line.ProductVendorId,
                    Discount = line.Discount,
                    DiscountPercentage = line.DiscountPercentage,
                    DetailStatus = line.DetailStatus,
                    CostRecived = line.CostRecived,
                    QuantityRecived = line.QuantityRecived,
                    TotalRecived = line.TotalRecived,
                    DiscountRecived = line.DiscountRecived,
                    TaxRecived = line.TaxRecived,
                    SubTotalRecived = line.SubTotalRecived
                }).ToList()
            };
        }
    }
}
