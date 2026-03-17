using cod.application.Common.Clases;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Inventory.Common;
using cod.application.Invoice.Queries.GetInvoices;
using cod.application.PurchaseOrder.Common;
using cod.application.PurchaseOrder.Queries.GetPurchasesOrder;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Inventory.Queries.GetInventories
{
    public class GetInventoriesQuery : BaseQuery, IGetInventoriesQuery
    {
        private readonly AuthService _authService;
        public GetInventoriesQuery(
            IRepository repository,
            AuthService authService) : base(repository)
        {
            _authService = authService;
        }

        public async Task<List<InventoryModel>> Execute(FilterInventory filter)
        {
            var timezone = _authService.ClientTimezone;

            Expression<Func<domain.Operation.Inventory, bool>> GetInventoryByCafeteriaFilter = GetInventoryByCafeteria(filter);
            Expression<Func<domain.Operation.Inventory, bool>> GetInventoryByDateFilter = GetInventoryByDate(filter, timezone);

            var baseQuery = _repository.Query<domain.Operation.Inventory>()
                //.Where(GetPurchaseOrderBystatusFilter)
                .Where(GetInventoryByCafeteriaFilter)
                .Where(GetInventoryByDateFilter)
                .Select(f => new
                {
                    f.Id,
                    f.CafeteriaId,
                    Cafeteria = f.Cafeteria.Name,
                    Register = f.RegisterId.HasValue ? f.Register.Name : string.Empty,
                    f.InventoryType,
                    f.RegisterId,
                    f.Total,
                    f.Date,
                    f.Reference,
                    f.CreatorUser,
                });

            return await baseQuery.Select(purchaseOrder => new InventoryModel
            {
                Id = purchaseOrder.Id,
                CafeteriaId = purchaseOrder.CafeteriaId,
                Cafeteria = purchaseOrder.Cafeteria,
                Register = purchaseOrder.Register,
                RegisterId = purchaseOrder.RegisterId,
                Total = purchaseOrder.Total,
                Date = purchaseOrder.Date.Date,
                Reference = purchaseOrder.Reference,
                CreatorUser = purchaseOrder.CreatorUser,
                InventoryType = purchaseOrder.InventoryType,
                InventoryTypeName = purchaseOrder.InventoryType.GetEnumDescription()
            }).ToListAsync();
        }

        public static Expression<Func<domain.Operation.Inventory, bool>> GetInventoryByCafeteria(FilterInventory filterModel)
        {
            if (filterModel.CafeteriaId.HasValue && filterModel.CafeteriaId > 0)
                return detail => detail.CafeteriaId == filterModel.CafeteriaId;
            return detail => true;
        }

        public static Expression<Func<domain.Operation.Inventory, bool>> GetInventoryByDate(FilterInventory filterModel, string timeZone)
        {
            filterModel.StartDate = DateService.GetCLientLocalTime(filterModel.StartDate.Value, timeZone);
            filterModel.EndDate = DateService.GetCLientLocalTime(filterModel.EndDate.Value, timeZone);

            if (filterModel.StartDate.HasValue && filterModel.EndDate.HasValue)
                return invoice => invoice.Date.Date >= filterModel.StartDate.Value.Date && invoice.Date.Date <= filterModel.EndDate.Value.Date;
            return invoice => true;
        }
    }
}
