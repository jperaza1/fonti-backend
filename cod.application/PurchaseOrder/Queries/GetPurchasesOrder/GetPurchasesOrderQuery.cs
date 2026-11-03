using cod.application.Common.Clases;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.PurchaseOrder.Common;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.PurchaseOrder.Queries.GetPurchasesOrder
{
    public class GetPurchasesOrderQuery : BaseQuery, IGetPurchasesOrderQuery
    {
        private readonly AuthService _authService;

        public GetPurchasesOrderQuery(IRepository repository, AuthService authService) : base(repository)
        {
            _authService = authService;
        }

        public async Task<List<PurchaseOrderModel>> Execute(FilterPurchaseOrder filter)
        {
            var timezone = _authService.ClientTimezone;
            var query = _repository.Query<domain.Operation.PurchaseOrder>()
                .Where(ByVendor(filter))
                .Where(ByStatus(filter))
                .Where(ByDate(filter, timezone));

            return await query.Select(item => new PurchaseOrderModel
            {
                Id = item.Id,
                VendorId = item.VendorId,
                Vendor = item.Vendor.Name,
                Total = item.Total,
                Date = item.Date.Date,
                Reference = item.Reference,
                OrderStatusName = item.OrderStatus.GetEnumDescription(),
                OrderStatus = item.OrderStatus,
                CreatorUser = item.CreatorUser,
                ApprovedUser = item.ApprovedUser,
                TypeInoice = item.TypeInoice,
                HasInvoice = false
            }).ToListAsync();
        }

        private static Expression<Func<domain.Operation.PurchaseOrder, bool>> ByVendor(FilterPurchaseOrder filter)
        {
            if (filter.VendorId > 0)
                return item => item.VendorId == filter.VendorId;
            return item => true;
        }

        private static Expression<Func<domain.Operation.PurchaseOrder, bool>> ByStatus(FilterPurchaseOrder filter)
        {
            if (filter.OrderStatus.IsValid())
                return item => item.OrderStatus == filter.OrderStatus;
            return item => true;
        }

        private static Expression<Func<domain.Operation.PurchaseOrder, bool>> ByDate(FilterPurchaseOrder filter, string timeZone)
        {
            if (!filter.StartDate.HasValue || !filter.EndDate.HasValue)
                return item => true;

            var start = DateService.GetCLientLocalTime(filter.StartDate.Value, timeZone).Date;
            var end = DateService.GetCLientLocalTime(filter.EndDate.Value, timeZone).Date;
            return item => item.Date.Date >= start && item.Date.Date <= end;
        }
    }
}
