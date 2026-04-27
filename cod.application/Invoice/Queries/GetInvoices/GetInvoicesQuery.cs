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

            Expression<Func<domain.Operation.Invoice, bool>> GetInvoiceByInvoiceDateFilter = GetInvoiceByInvoiceDate(filterModel, timezone);
            Expression<Func<domain.Operation.Invoice, bool>> GetInvoiceByInvoiceTypeFilter = GetInvoiceByInvoiceType(filterModel);
            Expression<Func<domain.Operation.Invoice, bool>> GetInvoiceByPaymentTypeFilter = GetInvoiceByPaymentType(filterModel);
            Expression<Func<domain.Operation.Invoice, bool>> GetInvoiceByPaymentStatusFilter = GetInvoiceByPaymentStatus(filterModel);
            Expression<Func<domain.Operation.Invoice, bool>> GetInvoiceByCafeteriaFilter = GetInvoiceByCafeteria(filterModel);
            Expression<Func<domain.Operation.Invoice, bool>> GetInvoiceByVendorFilter = GetInvoiceByVendor(filterModel);

            var baseQuery = _repository.Query<domain.Operation.Invoice>()
                .Where(GetInvoiceByInvoiceDateFilter)
                .Where(GetInvoiceByInvoiceTypeFilter)
                .Where(GetInvoiceByPaymentTypeFilter)
                .Where(GetInvoiceByPaymentStatusFilter)
                .Where(GetInvoiceByCafeteriaFilter)
                .Where(GetInvoiceByVendorFilter)
                .Select(f => new
                {
                    f.Id,
                    f.InvoiceDate,
                    f.InvoiceNumber,
                    f.InvoiceType,
                    f.PaymentStatus,
                    f.PaymentType,
                    f.PurchaseOrderId,
                    PurchaseOrder = f.PurchaseOrderId.HasValue ? f.PurchaseOrder.Reference : string.Empty,
                    Vendor = f.VendorId.HasValue ? f.Vendor.Name : string.Empty,
                    f.VendorId,
                    f.Description,
                    Cafeteria = f.Cafeteria.Name,
                    f.CafeteriaId,
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
                PurchaseOrderId = f.PurchaseOrderId,
                PurchaseOrder = f.PurchaseOrder,
                Vendor = f.Vendor,
                InvoiceNumber = f.InvoiceNumber,
                Description = f.Description,
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
                InvoiceType = f.InvoiceType,
                InvoiceTypeName = f.InvoiceType.GetEnumDescription(),
                PaymentStatus = f.PaymentStatus,
                PaymentStatusName = f.PaymentStatus.GetEnumDescription(),
                RemainingDays = f.RemainingDays,
                Cafeteria = f.Cafeteria,
                CafeteriaId = f.CafeteriaId,
                VendorId = f.VendorId
            }).ToListAsync();
        }

        public static Expression<Func<domain.Operation.Invoice, bool>> GetInvoiceByInvoiceType(FilterInvoiceModel filterModel)
        {
            if(filterModel.InvoiceType.IsValid())
                return invoice => invoice.InvoiceType == filterModel.InvoiceType;
            return invoice => true;
        }

        public static Expression<Func<domain.Operation.Invoice, bool>> GetInvoiceByPaymentType(FilterInvoiceModel filterModel)
        {
            if (filterModel.PaymentType.IsValid())
                return invoice => invoice.PaymentType == filterModel.PaymentType;
            return invoice => true;
        }

        public static Expression<Func<domain.Operation.Invoice, bool>> GetInvoiceByPaymentStatus(FilterInvoiceModel filterModel)
        {
            if (filterModel.PaymentStatus.IsValid())
                return invoice => invoice.PaymentStatus == filterModel.PaymentStatus;
            return invoice => true;
        }

        public static Expression<Func<domain.Operation.Invoice, bool>> GetInvoiceByCafeteria(FilterInvoiceModel filterModel)
        {
            if (filterModel.CafeteriaId > 0)
                return invoice => invoice.CafeteriaId == filterModel.CafeteriaId;
            return invoice => true;
        }

        public static Expression<Func<domain.Operation.Invoice, bool>> GetInvoiceByVendor(FilterInvoiceModel filterModel)
        {
            if (filterModel.VendorId > 0)
                return invoice => invoice.VendorId == filterModel.VendorId;
            return invoice => true;
        }

        public static Expression<Func<domain.Operation.Invoice, bool>> GetInvoiceByInvoiceDate(FilterInvoiceModel filterModel, string timeZone)
        {
            filterModel.DateFrom = DateService.GetCLientLocalTime(filterModel.DateFrom.Value, timeZone);
            filterModel.DateTo = DateService.GetCLientLocalTime(filterModel.DateTo.Value, timeZone);
            if (filterModel.DateFrom.HasValue && filterModel.DateTo.HasValue)
                return invoice => invoice.InvoiceDate.Date >= filterModel.DateFrom.Value.Date && invoice.InvoiceDate.Date <= filterModel.DateTo.Value.Date;
            return invoice => true;
        }
    }
}
