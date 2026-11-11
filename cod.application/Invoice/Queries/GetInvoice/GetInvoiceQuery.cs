using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Invoice.Common;
using cod.application.Payment.Common;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Invoice.Queries.GetInvoice
{
    public class GetInvoiceQuery : BaseQuery, IGetInvoiceQuery
    {
        public GetInvoiceQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<InvoiceModel> Execute(int id)
        {
            var baseQuery = _repository.Query<domain.Operation.ExpenseInvoice>()
                .Where(f => f.Id == id)
                .Select(f => new
                {
                    f.Id,
                    f.InvoiceDate,
                    f.InvoiceNumber,
                    f.Description,
                    f.PaymentStatus,
                    f.PaymentType,
                    CategoryId = f.Expense != null ? f.Expense.CategoryId : 0,
                    Category = f.Expense != null ? f.Expense.Category.Name : string.Empty,
                    Vendor = f.Expense != null && f.Expense.VendorId.HasValue
                        ? f.Expense.Vendors.Name
                        : (f.PurchaseOrder != null ? f.PurchaseOrder.Vendor.Name : string.Empty),
                    TruckName = f.Expense != null && f.Expense.TruckId.HasValue ? f.Expense.Truck.Plate : string.Empty,
                    TruckId = f.Expense != null ? f.Expense.TruckId : null,
                    VendorId = f.Expense != null ? f.Expense.VendorId : (f.PurchaseOrder != null ? (int?)f.PurchaseOrder.VendorId : null),
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
                    f.FileUploadedAt,
                    f.FileUploadedUser,
                    f.DocumentId,
                    RouteId = f.Expense != null ? f.Expense.RouteId : 0,
                    f.IsRefund,
                    RouteName = f.Expense != null ? f.Expense.Routes.Name : string.Empty,
                    RemainingDays = f.Balance == 0 ? 0 : f.DueDate.HasValue ? (f.DueDate.Value.Date - Clock.Now.Date).TotalDays : 0,
                    Payments = f.Payments.Select(x => new
                    {
                        x.Id,
                        x.PaymentDate,
                        x.Amount,
                        x.PaymentMethod
                    })
                });

            return await baseQuery.Select(f => new InvoiceModel
            {
                Id = f.Id,
                VendorName = f.Vendor,
                InvoiceNumber = f.InvoiceNumber,
                Description = f.Description,
                InvoiceDate = f.InvoiceDate,
                TaxableAmount = f.TaxableAmount,
                ExemptAmount = f.ExemptAmount,
                Discount = f.Discount,
                SubTotal = f.SubTotal,
                TaxAmount = f.TaxAmount,
                Total = f.Total,
                CategoryId = f.CategoryId,
                Category = f.Category,
                Balance = f.Balance,
                DueDate = f.DueDate,
                Paid = f.Paid,
                PaidAt = f.PaidAt,
                PaidUser = f.PaidUser,
                PaymentType = f.PaymentType,
                PaymentTypeName = f.PaymentType.GetEnumDescription(),
                PaymentStatus = f.PaymentStatus,
                PaymentStatusName = f.PaymentStatus.GetEnumDescription(),
                RemainingDays = f.RemainingDays,
                VendorId = f.VendorId,
                DocumentId = f.DocumentId,
                HasFile = f.DocumentId.HasValue && f.DocumentId > 0,
                FileUploadedUser = f.FileUploadedUser,
                FileUploadedAt = f.FileUploadedAt,
                Route = f.RouteName,
                RouteId = f.RouteId,
                IsRefund = f.IsRefund,
                TruckName = f.TruckName,
                TruckId = f.TruckId,
                Payments = f.Payments.Select(x => new InvoicePaymentModel
                {
                    InvoiceId = x.Id,
                    PaymentMethod = x.PaymentMethod,
                    PaymentMethodName = x.PaymentMethod.GetEnumDescription(),
                    Amount =x.Amount,
                    PaymentDate = x.PaymentDate
                }).ToList()
            }).FirstOrDefaultAsync();
        }
    }
}
