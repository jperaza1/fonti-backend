using cod.application.Common.Clases;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Payment.Commands.SendEmailNotification;
using cod.application.Payment.Common;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Payment.Queries.GetPaymentById
{
    public class GetPaymentByIdQuery : BaseQuery, IGetPaymentByIdQuery
    {
        private readonly AuthService _authService;
        public GetPaymentByIdQuery(
            IRepository repository, 
            AuthService authService) : base(repository)
        {
            this._authService = authService;
        }

        public async Task<PaymentModel> Execute(int id)
        {

            var baseQuery = _repository.QueryActive<domain.Finance.Payment>()
                .Where(g =>  g.Id == id);

            var result = await baseQuery.Select(g => new  PaymentModel
            {
                Id = g.Id,
                Reference = g.Reference,
                PaymentStatus = g.PaymentStatus,
                PaymentStatusName = g.PaymentStatus.GetEnumDescription(),
                Amount = g.Amount,
                InvoiceCount = g.InvoiceCount,
                BankReference = g.BankReference,
                Notes = g.Notes,
                PaymentDate = g.PaymentDate,
                BankAccount = g.BankAccount.Name,
                BankAccountId = g.BankAccountId,
                Details = g.Details.Select(x => new PaymentDetailModel
                {
                    Id = x.Id,
                    PaymentId = x.PaymentId,
                    InvoiceId = x.InvoiceId,
                    Amount = x.Amount,
                    VendorId = x.Invoice.Expense.VendorId.Value,
                    VendorName = x.Invoice.Expense.Vendors.Name,
                    InvoiceDate = x.Invoice.InvoiceDate,
                    InvoiceNumber = x.Invoice.InvoiceNumber,
                    InvoiceNote = x.Invoice.Expense.Notes,
                    Description = x.Description,
                    BankName = x.Invoice.Expense.VendorId.HasValue ? x.Invoice.Expense.Vendors.Bank.Name : string.Empty,
                    BankAccount = x.Invoice.Expense.VendorId.HasValue ? x.Invoice.Expense.Vendors.BankAccount : string.Empty,
                    DestinationBranch = x.Invoice.Expense.VendorId.HasValue ? x.Invoice.Expense.Vendors.C1 : string.Empty,
                    DestinationOffice = x.Invoice.Expense.VendorId.HasValue ? x.Invoice.Expense.Vendors.C2 : string.Empty,
                    BankAccountHolder = x.Invoice.Expense.VendorId.HasValue ? x.Invoice.Expense.Vendors.BankAccountHolder : string.Empty,
                }).ToList()
            }).FirstOrDefaultAsync();

            return result;
        }
    }
}
