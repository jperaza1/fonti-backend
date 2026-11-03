using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Payment.Commands.DeletePayment
{
    public class DeletePaymentCommand : BaseCommand, IDeletePaymentCommand
    {
        public DeletePaymentCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(int id)
        {
            var details = await _repository.Query<domain.Finance.PaymentDetail>()
                .Where(f => f.PaymentId == id)
                .ToListAsync();

            var payment = await _repository.Query<domain.Finance.Payment>()
                .Where(f => f.Id == id)
                .FirstOrDefaultAsync();

            foreach(var item in details)
            {
                await _repository.UpdateAsync<domain.Operation.ExpenseInvoice>(
                type => type.Id == item.InvoiceId,
                type => new domain.Operation.ExpenseInvoice
                {
                    PaymentId = null
                });

                _repository.DeleteSync(item);
            }

            _repository.DeleteSync(payment);

            await _repository.CommitAsync();
        }
    }
}
