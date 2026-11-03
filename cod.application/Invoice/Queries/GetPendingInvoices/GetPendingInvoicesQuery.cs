using cod.application.Common.Clases;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Invoice.Common;
using cod.application.Invoice.Queries.GetInvoices;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Invoice.Queries.GetPendingInvoices
{
    public class GetPendingInvoicesQuery : BaseQuery, IGetPendingInvoicesQuery
    {
        private readonly AuthService _authService;
        public GetPendingInvoicesQuery(IRepository repository, AuthService authService) : base(repository)
        {
            _authService = authService;
        }

        public async Task<List<InvoiceModel>> Execute(GetPendingInvoicesModel filterModel)
        {
            var timezone = _authService.ClientTimezone;
            var now = DateService.GetCLientLocalTime(Clock.Now, timezone);

            Expression<Func<domain.Operation.ExpenseInvoice, bool>> GetInvoiceByInvoiceDateFilter = GetInvoiceByInvoiceDate(filterModel, timezone);

            var baseQuery = _repository.Query<domain.Operation.ExpenseInvoice>()
                .Where(GetInvoiceByInvoiceDateFilter)
                .Where(g => !g.PaymentId.HasValue)
                .Where(g => g.PaymentType == domain.Features.Enums.DomainEnums.PaymentType.Credit)
                .Where(g => g.PaymentStatus != domain.Features.Enums.DomainEnums.PaymentStatus.Paid)
                .Select(f => new
                {
                    f.Id,
                    f.InvoiceDate,
                    f.InvoiceNumber,
                    f.PaymentStatus,
                    f.PaymentType,
                    Vendor = f.Expense.VendorId.HasValue ? f.Expense.Vendors.Name : string.Empty,
                    BankName = f.Expense.VendorId.HasValue ? f.Expense.Vendors.Bank.Name : string.Empty,
                    BankAccount = f.Expense.VendorId.HasValue ? f.Expense.Vendors.BankAccount : string.Empty,
                    f.Expense.VendorId,
                    f.Expense.Notes,
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
                    RemainingDays = f.DueDate.HasValue ? (f.DueDate.Value.Date - now.Date).TotalDays : 0,
                });

            return await baseQuery.Select(f => new InvoiceModel
            {
                Id = f.Id,
                VendorName = f.Vendor,
                InvoiceNumber = f.InvoiceNumber,
                Description = f.Notes,
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
                BankAccount = f.BankAccount,
                BankName = f.BankName,
            }).ToListAsync();
        }


        public static Expression<Func<domain.Operation.ExpenseInvoice, bool>> GetInvoiceByInvoiceDate(GetPendingInvoicesModel filterModel, string timeZone)
        {
            filterModel.StartDate = DateService.GetCLientLocalTime(filterModel.StartDate.Value, timeZone);
            filterModel.EndDate = DateService.GetCLientLocalTime(filterModel.EndDate.Value, timeZone);
            if (filterModel.StartDate.HasValue && filterModel.EndDate.HasValue)
                return invoice => invoice.InvoiceDate.Date >= filterModel.StartDate.Value.Date && invoice.InvoiceDate.Date <= filterModel.EndDate.Value.Date;
            return invoice => true;
        }
    }
}
