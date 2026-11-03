using cod.application.Common.Clases;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Payment.Common;
using cod.domain.Features;
using DocuSign.eSign.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace cod.application.Payment.Queries.GetAllPayment
{
    public class GetAllPaymentQuery : BaseQuery, IGetAllPaymentQuery
    {
        private readonly AuthService _authService;
        public GetAllPaymentQuery(
            IRepository repository, 
            AuthService authService) : base(repository)
        {
            _authService = authService;
        }

        public async Task<List<PaymentModel>> Execute(FilterPaymentModel model)
        {
            var timezone = _authService.ClientTimezone;
            Expression<Func<domain.Finance.Payment, bool>> GetPaymentByDateFilter = GetPaymentByDate(model, timezone);

            var result = await _repository.Query<domain.Finance.Payment>()
                .Where(GetPaymentByDateFilter)
                .Select(f => new PaymentModel
                {
                    Id = f.Id,
                    Reference = f.Reference,
                    PaymentDate = f.PaymentDate,
                    BankAccount = f.BankAccount.Name,
                    BankAccountId = f.BankAccountId,
                    Amount = f.Amount,
                    InvoiceCount = f.InvoiceCount,
                    BankReference = f.BankReference,
                    Notes = f.Notes,
                    PaymentStatus = f.PaymentStatus,
                    PaymentStatusName = f.PaymentStatus.GetEnumDescription(),
                    Details = new List<PaymentDetailModel>()
                }).ToListAsync();

            return result;
        }

        public static Expression<Func<domain.Finance.Payment, bool>> GetPaymentByDate(FilterPaymentModel filterModel, string timeZone)
        {
            filterModel.StartDate = DateService.GetCLientLocalTime(filterModel.StartDate.Value, timeZone);
            filterModel.EndDate = DateService.GetCLientLocalTime(filterModel.EndDate.Value, timeZone);

            if (filterModel.StartDate.HasValue && filterModel.EndDate.HasValue)
                return payment => payment.PaymentDate.Date >= filterModel.StartDate.Value.Date && payment.PaymentDate.Date <= filterModel.EndDate.Value.Date;
            return payment => true;
        }
    }
}
