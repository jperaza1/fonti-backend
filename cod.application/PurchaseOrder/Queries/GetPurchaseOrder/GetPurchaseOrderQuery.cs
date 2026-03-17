using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Product.Common;
using cod.application.PurchaseOrder.Common;
using cod.domain.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var baseQuery = _repository.Query<domain.Operation.PurchaseOrder>()
                .Where(f => f.Id == id)
                .Select(f => new
            {
                f.Id,
                f.VendorId,
                Vendor = f.Vendor.Name,
                f.Vendor.Rtn,
                f.Vendor.Seller,
                f.Vendor.Address,
                f.Vendor.Phone,
                f.CafeteriaId,
                Cafeteria = f.Cafeteria.Name,
                f.OrderStatus,
                f.Total,
                f.Date,
                f.Reference,
                f.ReasonRejection,
                f.ReasonCanceled,
                f.CanceledAt,
                f.CanceledUser,
                f.TypeInoice,
                PurchaseOrderDetails = f.PurchaseOrderDetails.Select(x => new
                {
                    x.Id,
                    x.PurchaseOrderId,
                    x.ProductId,
                    Product = $"{x.Product.Name} ({x.Product.Measure.Name})",
                    x.Cost,
                    x.Quantity,
                    x.Discount,
                    x.DiscountPercentage,
                    x.Tax,
                    x.TaxPercentage,
                    x.SubTotal,
                    x.Total,
                    x.QuantityRecived,
                    x.CostRecived,
                    x.DetailStatus,
                    x.TotalRecived,
                    x.DiscountRecived,
                    x.TaxRecived,
                    x.SubTotalRecived
                }).ToList()
            });

            var purchaseOrder = await baseQuery.FirstOrDefaultAsync();

            if (purchaseOrder == null)
                throw new ApplicationException("Orden no existe", "ORDEN COMPRA");

            return new PurchaseOrderModel
            {
                Id = purchaseOrder.Id,
                VendorId = purchaseOrder.VendorId,
                Vendor = purchaseOrder.Vendor,
                Rtn = purchaseOrder.Rtn,
                Address = purchaseOrder.Address,
                Phone = purchaseOrder.Phone,
                Seller = purchaseOrder.Seller,
                CafeteriaId = purchaseOrder.CafeteriaId,
                Cafeteria = purchaseOrder.Cafeteria,
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
                Details = purchaseOrder.PurchaseOrderDetails.Select(f => new PurchaseOrderDetailModel
                {
                    Id = f.Id,
                    PurchaseOrderId = f.PurchaseOrderId,
                    Cost = f.Cost,
                    Quantity = f.Quantity,
                    Tax = f.Tax,
                    TaxPercentage = f.TaxPercentage,
                    SubTotal = f.SubTotal,
                    Total = f.Total,
                    Product = f.Product,
                    ProductId = f.ProductId,
                    Discount = f.Discount,
                    DiscountPercentage = f.DiscountPercentage,
                    DetailStatus = f.DetailStatus,
                    CostRecived = f.CostRecived,
                    QuantityRecived = f.QuantityRecived,
                    TotalRecived = f.TotalRecived,
                    SubTotalRecived = f.SubTotalRecived,
                    DiscountRecived = f.DiscountRecived,
                    TaxRecived = f.TaxRecived,
                }).ToList()
            };
        }
    }
}
