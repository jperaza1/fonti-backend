using cod.application.Common.Clases;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.PurchaseOrder.Queries.GetPurchasesOrder;
using cod.domain.Features;
using DocuSign.eSign.Model;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.PurchaseOrder.Queries.GetReportExpense
{
    public class GetReportExpenseQuery : BaseQuery, IGetReportExpenseQuery
    {
        private readonly AuthService _authService;
        public GetReportExpenseQuery(
            IRepository repository,
            AuthService authService) : base(repository)
        {
            _authService = authService;
        }

        public async Task<List<ReportExpenseModel>> Execute(ReportExpenseFilterModel model)
        {
            var timezone = _authService.ClientTimezone;

            Expression<Func<domain.Operation.PurchaseOrder, bool>> GetPurchaseOrderByCafeteriaFilter = GetPurchaseOrderByCafeteria(model);
            Expression<Func<domain.Operation.PurchaseOrder, bool>> GetPurchaseOrderByDateFilter = GetPurchaseOrderByDate(model, timezone);
            Expression<Func<domain.Operation.PurchaseOrder, bool>> GetPurchaseOrderByVendorsFilter = GetPurchaseOrderByVendors(model);

            var response = new List<ReportExpenseModel>();

            var listPurchaseOrder = await _repository.Query<domain.Operation.PurchaseOrder>()
                .Where(GetPurchaseOrderByVendorsFilter)
                .Where(GetPurchaseOrderByCafeteriaFilter)
                .Where(GetPurchaseOrderByDateFilter)
                .Where(f => 
                    f.OrderStatus == domain.Features.Enums.DomainEnums.PurchaseOrderStatus.Completed || 
                    f.OrderStatus == domain.Features.Enums.DomainEnums.PurchaseOrderStatus.Review)
                .Select(f => new
                {
                    f.Id,
                    f.VendorId,
                    Vendor = f.Vendor.Name,
                    f.Date,
                    Total = f.PurchaseOrderDetails.Sum(f => f.TotalRecived)
                }).ToListAsync();

            var datesInRange =
                model.FilterType == FilterType.Weekly ? model.StartDate.Value.Date.WeeksInRange(model.EndDate.Value.Date) :
                model.StartDate.Value.Date.MonthsInRange(model.EndDate.Value.Date);

            var groupedList = listPurchaseOrder.GroupBy(data => data.Vendor);

            foreach(var group in groupedList)
            {
                var row = new ReportExpenseModel()
                {
                    Vendor = group.Key,
                    DateMoves = new Dictionary<string, double>(),
                    Total = 0,
                };

                foreach (var date in datesInRange)
                {
                    var purchases =
                            model.FilterType == FilterType.Weekly ? group.Where(g => g.Date.Date >= date.Date.Date && g.Date.Date <= date.Date.AddDays(6).Date).ToList() :
                            group.Where(data => data.Date.Month == date.Month && data.Date.Year == date.Year).ToList();

                    row.DateMoves.Add(
                            model.FilterType == FilterType.Weekly ? date.ToFormatDate() : date.ToFormatMonthYear(), purchases.Sum(f => f.Total));

                    row.Total += purchases.Sum(g => g.Total);
                }

                response.Add(row);
            }

            return response;

        }

        public static Expression<Func<domain.Operation.PurchaseOrder, bool>> GetPurchaseOrderByCafeteria(ReportExpenseFilterModel filterModel)
        {
            if (filterModel.CafeteriaId >= 0)
                return detail => detail.CafeteriaId == filterModel.CafeteriaId;
            return detail => true;
        }

        public static Expression<Func<domain.Operation.PurchaseOrder, bool>> GetPurchaseOrderByVendors(ReportExpenseFilterModel filterModel)
        {
            if (filterModel.Vendors.Any())
                return detail => filterModel.Vendors.Any(g => g == detail.VendorId);
            return detail => true;
        }

        public static Expression<Func<domain.Operation.PurchaseOrder, bool>> GetPurchaseOrderByDate(ReportExpenseFilterModel filterModel, string timeZone)
        {
            filterModel.StartDate = DateService.GetCLientLocalTime(filterModel.StartDate.Value, timeZone);
            filterModel.EndDate = DateService.GetCLientLocalTime(filterModel.EndDate.Value, timeZone);

            if (filterModel.StartDate.HasValue && filterModel.EndDate.HasValue)
                return invoice => invoice.Date.Date >= filterModel.StartDate.Value.Date && invoice.Date.Date <= filterModel.EndDate.Value.Date;
            return invoice => true;
        }
    }
}
