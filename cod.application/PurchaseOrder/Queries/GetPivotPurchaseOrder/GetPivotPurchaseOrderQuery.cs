using cod.application.Common.Clases;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.PurchaseOrder.Queries.GetPurchasesOrder;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
            var query = _repository.Query<domain.Operation.PurchaseOrderDetail>()
                .Where(item => item.IsActive && item.PurchaseOrder.IsActive);

            if (filter.VendorId > 0)
                query = query.Where(item => item.PurchaseOrder.VendorId == filter.VendorId);

            if (filter.OrderStatus.IsValid())
                query = query.Where(item => item.PurchaseOrder.OrderStatus == filter.OrderStatus);

            if (filter.StartDate.HasValue && filter.EndDate.HasValue)
            {
                var start = DateService.GetCLientLocalTime(filter.StartDate.Value, _authService.ClientTimezone).Date;
                var end = DateService.GetCLientLocalTime(filter.EndDate.Value, _authService.ClientTimezone).Date;
                query = query.Where(item => item.PurchaseOrder.Date.Date >= start && item.PurchaseOrder.Date.Date <= end);
            }

            return await query.Select(item => new GetPivotPurchaseOrderModel
            {
                Id = item.Id,
                Reference = item.PurchaseOrder.Reference,
                Date = item.PurchaseOrder.Date.Date,
                Vendor = item.PurchaseOrder.Vendor.Name,
                Product = item.ProductVendorId.HasValue ? item.ProductVendor.Product.Name : item.Product.Name,
                QuantityRecived = item.QuantityRecived,
                CostRecived = item.CostRecived,
                TotalRecived = item.TotalRecived
            }).ToListAsync();
        }
    }
}
