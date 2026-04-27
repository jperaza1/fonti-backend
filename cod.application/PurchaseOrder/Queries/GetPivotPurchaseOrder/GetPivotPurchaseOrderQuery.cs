using cod.application.Common.Clases;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.PurchaseOrder.Common;
using cod.application.PurchaseOrder.Queries.GetPurchasesOrder;
using cod.domain.Features;
using cod.domain.Features.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.PurchaseOrder.Queries.GetPivotPurchaseOrder
{
    public class GetPivotPurchaseOrderQuery : BaseQuery, IGetPivotPurchaseOrderQuery
    {
        private readonly AuthService _authService;
        public GetPivotPurchaseOrderQuery(IRepository repository, AuthService authService) : base(repository)
        {
            _authService = authService;
        }

        public async Task<List<GetPivotPurchaseOrderModel>> Execute(FilterPurchaseOrder filter)
        {
            var timezone = _authService.ClientTimezone;

            Expression<Func<domain.Operation.PurchaseOrderDetail, bool>> GetPurchaseOrderBystatusFilter = GetPurchaseOrderByOrderStatus(filter);
            Expression<Func<domain.Operation.PurchaseOrderDetail, bool>> GetPurchaseOrderByCafeteriaFilter = GetPurchaseOrderByCafeteria(filter);
            Expression<Func<domain.Operation.PurchaseOrderDetail, bool>> GetPurchaseOrderByDateFilter = GetPurchaseOrderByDate(filter, timezone);
            Expression<Func<domain.Operation.PurchaseOrderDetail, bool>> GetInvoiceByVendorFilter = GetPurchaseOrderByVendor(filter);

            var baseQuery = await _repository.Query<domain.Operation.PurchaseOrderDetail>()
                .Where(GetPurchaseOrderByCafeteriaFilter)
                .Where(GetPurchaseOrderByDateFilter)
                .Where(GetInvoiceByVendorFilter)
                .Where(GetPurchaseOrderBystatusFilter)
                .Select(f => new GetPivotPurchaseOrderModel
                {
                    Id = f.Id,
                    Reference = f.PurchaseOrder.Reference,
                    Date = f.PurchaseOrder.Date,
                    Vendor = f.PurchaseOrder.Vendor.Name,
                    Cafeteria = f.PurchaseOrder.Cafeteria.Name,
                    Product = f.Product.Name,
                    QuantityRecived = f.QuantityRecived,
                    CostRecived = f.CostRecived,
                    TotalRecived = f.TotalRecived,
                }).ToListAsync();

            return baseQuery;
        }

        public static Expression<Func<domain.Operation.PurchaseOrderDetail, bool>> GetPurchaseOrderByOrderStatus(FilterPurchaseOrder filterModel)
        {
            if (filterModel.OrderStatus.IsValid())
                return invoice => invoice.PurchaseOrder.OrderStatus == DomainEnums.PurchaseOrderStatus.Completed;
            return invoice => true;
        }

        public static Expression<Func<domain.Operation.PurchaseOrderDetail, bool>> GetPurchaseOrderByCafeteria(FilterPurchaseOrder filterModel)
        {
            if (filterModel.CafeteriaId.HasValue && filterModel.CafeteriaId > 0)
                return detail => detail.PurchaseOrder.CafeteriaId == filterModel.CafeteriaId;
            return detail => true;
        }

        public static Expression<Func<domain.Operation.PurchaseOrderDetail, bool>> GetPurchaseOrderByVendor(FilterPurchaseOrder filterModel)
        {
            if (filterModel.VendorId > 0)
                return detail => detail.PurchaseOrder.VendorId == filterModel.VendorId;
            return detail => true;
        }

        public static Expression<Func<domain.Operation.PurchaseOrderDetail, bool>> GetPurchaseOrderByDate(FilterPurchaseOrder filterModel, string timeZone)
        {
            filterModel.StartDate = DateService.GetCLientLocalTime(filterModel.StartDate.Value, timeZone);
            filterModel.EndDate = DateService.GetCLientLocalTime(filterModel.EndDate.Value, timeZone);

            if (filterModel.StartDate.HasValue && filterModel.EndDate.HasValue)
                return invoice => invoice.PurchaseOrder.Date.Date >= filterModel.StartDate.Value.Date && invoice.PurchaseOrder.Date.Date <= filterModel.EndDate.Value.Date;
            return invoice => true;
        }
    }
}
