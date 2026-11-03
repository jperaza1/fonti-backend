using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Payment.Common;
using cod.domain.Features;
using cod.domain.Finance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Payment.Commands.UpdatePayment
{
    public class UpdatePaymentCommand : BaseCommand, IUpdatePaymentCommand
    {
        public UpdatePaymentCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(int id, PaymentModel model)
        {
            var paymentDetail = await _repository.Query<domain.Finance.PaymentDetail>()
                .Where(x => x.PaymentId == id)
                .ToListAsync();

            var payment = await _repository.Query<domain.Finance.Payment>()
                .Where(f => f.Id == id)
                .FirstOrDefaultAsync();

            var detailsToRemove = paymentDetail.Where(detail => model.Details.All(member => member.Id != detail.Id)).ToList();
            var detailsToUpdate = model.Details.Where(f => f.Id > 0).ToList();
            var detailsToCreate = model.Details.Where(f => f.Id <= 0).ToList();

            foreach(var item in detailsToCreate)
            {
                _repository.InsertAsync(new PaymentDetail
                {
                    PaymentId = payment.Id,
                    InvoiceId = item.InvoiceId,
                    Amount = Math.Round(item.Amount, 2),
                    Description = item.Description
                });

                await _repository.UpdateAsync<domain.Operation.ExpenseInvoice>(
                f => f.Id == item.InvoiceId,
                f => new domain.Operation.ExpenseInvoice
                {
                    PaymentId = payment.Id,
                });
            }

            foreach (var item in detailsToRemove)
            {
                await _repository.UpdateAsync<domain.Operation.ExpenseInvoice>(
                type => type.Id == item.InvoiceId,
                type => new domain.Operation.ExpenseInvoice
                {
                    PaymentId = null
                });

                _repository.DeleteSync(item);
            }

            foreach (var item in detailsToUpdate)
            {
                var detail = paymentDetail.FirstOrDefault(g => g.Id == item.Id);

                if(detail != null)
                {
                    detail.Description = item.Description;
                    _repository.UpdateSync(detail);
                }
            }

            payment.InvoiceCount = model.Details.Count;
            payment.Amount = model.Details.Sum(g => g.Amount);
            payment.BankAccountId = model.BankAccountId;

            _repository.UpdateSync(payment);

            await _repository.CommitAsync();
        }
    }
}
