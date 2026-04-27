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
            var baseQuery = _repository.Query<domain.Operation.Invoice>()
                .Where(f => f.Id == id)
                .Select(f => new
                {
                    f.Id,
                    f.InvoiceDate,
                    f.InvoiceNumber,
                    f.PaymentStatus,
                    f.Description,
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
                InvoiceNumber = f.InvoiceNumber,
                Description = f.Description,
                InvoiceDate = f.InvoiceDate,
                TaxableAmount = f.TaxableAmount,
                ExemptAmount = f.ExemptAmount,
                Discount = f.Discount,
                SubTotal = f.SubTotal,
                TaxAmount = f.TaxAmount,
                Total = f.Total,
                Balance = f.Balance,
                DueDate = f.DueDate,
                Paid = f.Paid,
                PaidAt = f.PaidAt,
                PaidUser = f.PaidUser,
                PaymentStatus = f.PaymentStatus,
                PaymentStatusName = f.PaymentStatus.GetEnumDescription(),
                RemainingDays = f.RemainingDays,
                Payments = f.Payments.Select(x => new PaymentModel
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
