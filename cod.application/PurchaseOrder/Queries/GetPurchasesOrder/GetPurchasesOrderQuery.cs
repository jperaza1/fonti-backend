using cod.application.Common.Clases;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Invoice.Queries.GetInvoices;
using cod.application.PurchaseOrder.Common;
using cod.domain.Features;
using cod.domain.Features.Enums;
using cod.domain.Operation;
using DocuSign.eSign.Model;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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

            Expression<Func<domain.Operation.PurchaseOrder, bool>> GetPurchaseOrderBystatusFilter = GetPurchaseOrderBystatus(filter);
            Expression<Func<domain.Operation.PurchaseOrder, bool>> GetPurchaseOrderByCafeteriaFilter = GetPurchaseOrderByCafeteria(filter);
            Expression<Func<domain.Operation.PurchaseOrder, bool>> GetPurchaseOrderByDateFilter = GetPurchaseOrderByDate(filter, timezone);
            Expression<Func<domain.Operation.PurchaseOrder, bool>> GetInvoiceByVendorFilter = GetPurchaseOrderByVendor(filter);
            Expression<Func<domain.Operation.PurchaseOrder, bool>> GetPurchaseOrderByOrderStatusFilter = GetPurchaseOrderByOrderStatus(filter);


            
            var baseQuery = _repository.Query<domain.Operation.PurchaseOrder>()
                //.Where(GetPurchaseOrderBystatusFilter)
                .Where(GetPurchaseOrderByCafeteriaFilter)
                .Where(GetPurchaseOrderByDateFilter)
                .Where(GetInvoiceByVendorFilter)
                .Where(GetPurchaseOrderByOrderStatusFilter)
                .Select(f => new
                {
                    f.Id,
                    f.VendorId,
                    Vendor = f.Vendor.Name,
                    f.CafeteriaId,
                    Cafeteria = f.Cafeteria.Name,
                    f.Total,
                    f.Date,
                    f.Reference,
                    f.OrderStatus,
                    f.CreatorUser,
                    f.ApprovedUser,
                    f.TypeInoice,
                    HasInvoice = f.Invoices.Any()
                });

            return await baseQuery.Select(purchaseOrder => new PurchaseOrderModel
            {
                Id = purchaseOrder.Id,
                VendorId = purchaseOrder.VendorId,
                Vendor = purchaseOrder.Vendor,
                CafeteriaId = purchaseOrder.CafeteriaId,
                Cafeteria = purchaseOrder.Cafeteria,
                Total = purchaseOrder.Total,
                Date = purchaseOrder.Date.Date,
                Reference = purchaseOrder.Reference,
                OrderStatusName = purchaseOrder.OrderStatus.GetEnumDescription(),
                OrderStatus = purchaseOrder.OrderStatus,
                CreatorUser = purchaseOrder.CreatorUser,
                ApprovedUser = purchaseOrder.ApprovedUser,
                HasInvoice = purchaseOrder.HasInvoice,
                TypeInoice = purchaseOrder.TypeInoice,
            }).ToListAsync();
        }

        public static Expression<Func<domain.Operation.PurchaseOrder, bool>> GetPurchaseOrderBystatus(FilterPurchaseOrder filterModel)
        {
            return filterModel.UserType switch
            {
                DomainEnums.UserType.Supervisor => detail => detail.OrderStatus == PurchaseOrderStatus.Created || detail.OrderStatus == PurchaseOrderStatus.Completed || detail.OrderStatus == PurchaseOrderStatus.Review,
                DomainEnums.UserType.Bodega => detail => detail.OrderStatus == PurchaseOrderStatus.Created || detail.OrderStatus == PurchaseOrderStatus.Processing || detail.OrderStatus == PurchaseOrderStatus.Completed,
                DomainEnums.UserType.Compras => detail => detail.OrderStatus == PurchaseOrderStatus.Approved || detail.OrderStatus == PurchaseOrderStatus.Completed,
                _ => detail => true
            };
        }

        public static Expression<Func<domain.Operation.PurchaseOrder, bool>> GetPurchaseOrderByCafeteria(FilterPurchaseOrder filterModel)
        {
            if (filterModel.Cafeterias.Any())
                return detail => filterModel.Cafeterias.Any(f => f == detail.CafeteriaId);
            return detail => true;
        }

        public static Expression<Func<domain.Operation.PurchaseOrder, bool>> GetPurchaseOrderByVendor(FilterPurchaseOrder filterModel)
        {
            if (filterModel.VendorId > 0)
                return detail => detail.VendorId == filterModel.VendorId;
            return detail => true;
        }

        public static Expression<Func<domain.Operation.PurchaseOrder, bool>> GetPurchaseOrderByOrderStatus(FilterPurchaseOrder filterModel)
        {
            if (filterModel.OrderStatus.IsValid())
                return invoice => invoice.OrderStatus == filterModel.OrderStatus;
            return invoice => true;
        }

        public static Expression<Func<domain.Operation.PurchaseOrder, bool>> GetPurchaseOrderByDate(FilterPurchaseOrder filterModel, string timeZone)
        {
            filterModel.StartDate = DateService.GetCLientLocalTime(filterModel.StartDate.Value, timeZone);
            filterModel.EndDate = DateService.GetCLientLocalTime(filterModel.EndDate.Value, timeZone);

            if (filterModel.StartDate.HasValue && filterModel.EndDate.HasValue)
                return invoice => invoice.Date.Date >= filterModel.StartDate.Value.Date && invoice.Date.Date <= filterModel.EndDate.Value.Date;
            return invoice => true;
        }
    }
}
