using cod.application.Common.Clases;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Invoice.Common;
using cod.domain.Data;
using cod.domain.Features;
using cod.domain.Features.Enums;
using DocuSign.eSign.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.Invoice.Queries.GetInvoices
{
    public class GetInvoicesQuery : BaseQuery, IGetInvoicesQuery
    {
        private readonly AuthService _authService;
        public GetInvoicesQuery(IRepository repository, AuthService authService) : base(repository)
        {
            _authService = authService;
        }

        public async Task<List<InvoiceModel>> Execute(FilterInvoiceModel filterModel)
        {
            var timezone = _authService.ClientTimezone;
            var now = DateService.GetCLientLocalTime(Clock.Now, timezone);

            Expression<Func<domain.Operation.ExpenseInvoice, bool>> GetInvoiceByInvoiceDateFilter = GetInvoiceByInvoiceDate(filterModel, timezone);
            Expression<Func<domain.Operation.ExpenseInvoice, bool>> GetInvoiceByPaymentTypeFilter = GetInvoiceByPaymentType(filterModel);
            Expression<Func<domain.Operation.ExpenseInvoice, bool>> GetInvoiceByPaymentStatusFilter = GetInvoiceByPaymentStatus(filterModel);
            Expression<Func<domain.Operation.ExpenseInvoice, bool>> GetInvoiceByRouteFilter = GetInvoiceByRoute(filterModel);
            Expression<Func<domain.Operation.ExpenseInvoice, bool>> GetInvoiceByVendorFilter = GetInvoiceByVendor(filterModel);

            var baseQuery = _repository.Query<domain.Operation.ExpenseInvoice>()
                .Where(GetInvoiceByInvoiceDateFilter)
                .Where(GetInvoiceByPaymentTypeFilter)
                .Where(GetInvoiceByPaymentStatusFilter)
                .Where(GetInvoiceByRouteFilter)
                .Where(GetInvoiceByVendorFilter)
                .Select(f => new
                {
                    f.Id,
                    f.InvoiceDate,
                    f.InvoiceNumber,
                    f.PaymentStatus,
                    f.PaymentType,
                    f.Expense.RouteId,
                    RouteName = f.Expense.Routes.Name,
                    VendorName = f.Expense.VendorId.HasValue ? f.Expense.Vendors.Name : string.Empty,
                    f.Expense.VendorId,
                    f.TaxableAmount,
                    f.ExemptAmount,
                    f.Discount,
                    f.SubTotal,
                    f.Total,
                    f.TaxAmount,
                    f.DueDate,
                    f.Paid,
                    f.PaidAt,
                    f.PaidUser,
                    f.Balance,
                    ExpenseId = f.Expense.Id,
                    f.Expense.CategoryId,
                    Category = f.Expense.Category.Name,
                    RemainingDays = f.DueDate.HasValue ? (f.DueDate.Value.Date - now.Date).TotalDays : 0,
                    TruckName = f.Expense.TruckId.HasValue ? f.Expense.Truck.Plate : string.Empty,
                    f.Expense.TruckId,
                });

            return await baseQuery.Select(f => new InvoiceModel
            {
                Id = f.Id,
                ExpenseId = f.ExpenseId,
                VendorName = f.VendorName,
                InvoiceNumber = f.InvoiceNumber,
                InvoiceDate = f.InvoiceDate.Date,
                TaxableAmount = f.TaxableAmount,
                ExemptAmount = f.ExemptAmount,
                Discount = f.Discount,
                SubTotal = f.SubTotal,
                TaxAmount = f.TaxAmount,
                Total = f.Total,
                Balance = f.Balance,
                DueDate = f.DueDate.HasValue ? f.DueDate.Value.Date : null,
                Paid = f.Paid,
                PaidAt = f.PaidAt,
                PaidUser = f.PaidUser,
                PaymentType = f.PaymentType,
                PaymentTypeName = f.PaymentType.GetEnumDescription(),
                PaymentStatus = f.PaymentStatus,
                PaymentStatusName = f.PaymentStatus.GetEnumDescription(),
                RemainingDays = f.RemainingDays,
                VendorId = f.VendorId,
                Category = f.Category,
                CategoryId = f.CategoryId,
                RouteId = f.RouteId,
                Route = f.RouteName,
                TruckId = f.TruckId,
                TruckName = f.TruckName
            }).ToListAsync();
        }

        public static Expression<Func<domain.Operation.ExpenseInvoice, bool>> GetInvoiceByPaymentType(FilterInvoiceModel filterModel)
        {
            if (filterModel.PaymentType.IsValid())
                return invoice => invoice.PaymentType == filterModel.PaymentType;
            return invoice => true;
        }

        public static Expression<Func<domain.Operation.ExpenseInvoice, bool>> GetInvoiceByPaymentStatus(FilterInvoiceModel filterModel)
        {
            if (filterModel.PaymentStatus.IsValid())
                return invoice => invoice.PaymentStatus == filterModel.PaymentStatus;
            return invoice => true;
        }

        public static Expression<Func<domain.Operation.ExpenseInvoice, bool>> GetInvoiceByRoute(FilterInvoiceModel filterModel)
        {
            if (filterModel.RouteId > 0)
                return invoice => invoice.Expense.RouteId == filterModel.RouteId;
            return invoice => true;
        }

        public static Expression<Func<domain.Operation.ExpenseInvoice, bool>> GetInvoiceByVendor(FilterInvoiceModel filterModel)
        {
            if (filterModel.VendorId > 0)
                return invoice => invoice.Expense.VendorId == filterModel.VendorId;
            return invoice => true;
        }

        public static Expression<Func<domain.Operation.ExpenseInvoice, bool>> GetInvoiceByInvoiceDate(FilterInvoiceModel filterModel, string timeZone)
        {
            filterModel.DateFrom = DateService.GetCLientLocalTime(filterModel.DateFrom.Value, timeZone);
            filterModel.DateTo = DateService.GetCLientLocalTime(filterModel.DateTo.Value, timeZone);
            if (filterModel.DateFrom.HasValue && filterModel.DateTo.HasValue)
                return invoice => invoice.InvoiceDate.Date >= filterModel.DateFrom.Value.Date && invoice.InvoiceDate.Date <= filterModel.DateTo.Value.Date;
            return invoice => true;
        }
    }
}
