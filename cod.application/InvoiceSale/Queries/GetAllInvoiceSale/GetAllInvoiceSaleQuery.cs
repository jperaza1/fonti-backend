using cod.application.Common.Clases;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Invoice.Queries.GetInvoices;
using cod.application.InvoiceSale.Common;
using cod.domain.Features;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.InvoiceSale.Queries.GetAllInvoiceSale
{
    public class GetAllInvoiceSaleQuery : BaseQuery, IGetAllInvoiceSaleQuery
    {
        private readonly AuthService _authService;
        public GetAllInvoiceSaleQuery(
            IRepository repository,
            AuthService authService) : base(repository)
        {
            _authService = authService;
        }

        public async Task<List<SaleInvoiceModel>> Execute(FilterInvoiceSaleModel filter)
        {
            var timezone = _authService.ClientTimezone;

            Expression<Func<domain.Finance.SaleInvoice, bool>> GetInvoiceByInvoiceDateFilter = GetInvoiceByInvoiceDate(filter, timezone);
            Expression<Func<domain.Finance.SaleInvoice, bool>> GetInvoiceByCustomerFilter = GetInvoiceByCustomer(filter);

            var baseQuery = _repository.Query<domain.Finance.SaleInvoice>()
                .Where(GetInvoiceByInvoiceDateFilter)
                .Where(GetInvoiceByCustomerFilter)
                .Select(f => new
                {
                    f.Id,
                    f.InvoiceDate,
                    f.InvoiceNumber,
                    Customer = f.Customer.Name,
                    f.CustomerId,
                    f.PaymentStatus,
                    f.PaymentType,
                    f.SubTotal,
                    f.Total,
                    f.TaxAmount,
                    f.DueDate,
                    f.Paid,
                    f.PaidAt,
                    f.PaidUser,
                    f.Balance,
                });

            return await baseQuery.Select(f => new SaleInvoiceModel
            {
                Id = f.Id,
                InvoiceNumber = f.InvoiceNumber,
                InvoiceDate = f.InvoiceDate.Date,
                Customer = f.Customer,
                CustomerId = f.CustomerId,
                SubTotal = f.SubTotal,
                TaxAmount = f.TaxAmount,
                Total = f.Total,
                Balance = f.Balance,
                DueDate = f.DueDate.HasValue ? f.DueDate.Value.Date : null,
                Paid = f.Paid,
                PaidAt = f.PaidAt,
                PaidUser = f.PaidUser,
                PaymentType = f.PaymentType,
                PaymentTypeName = f.PaymentType.IsValid() ? f.PaymentType.GetEnumDescription() : string.Empty,
                PaymentStatus = f.PaymentStatus,
                PaymentStatusName = f.PaymentStatus.IsValid() ? f.PaymentStatus.GetEnumDescription() : string.Empty,
            }).ToListAsync();
        }

        public static Expression<Func<domain.Finance.SaleInvoice, bool>> GetInvoiceByInvoiceDate(FilterInvoiceSaleModel filterModel, string timeZone)
        {
            filterModel.DateFrom = DateService.GetCLientLocalTime(filterModel.DateFrom.Value, timeZone);
            filterModel.DateTo = DateService.GetCLientLocalTime(filterModel.DateTo.Value, timeZone);
            if (filterModel.DateFrom.HasValue && filterModel.DateTo.HasValue)
                return invoice => invoice.InvoiceDate.Date >= filterModel.DateFrom.Value.Date && invoice.InvoiceDate.Date <= filterModel.DateTo.Value.Date;
            return invoice => true;
        }

        public static Expression<Func<domain.Finance.SaleInvoice, bool>> GetInvoiceByCustomer(FilterInvoiceSaleModel filterModel)
        {
            if (filterModel.CustomerId > 0)
                return detail => detail.CustomerId == filterModel.CustomerId;
            return detail => true;
        }
    }
}
