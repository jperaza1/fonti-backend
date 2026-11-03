using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Invoice.Commands.DeleteInvoice
{
    public class DeleteInvoiceCommand : BaseCommand, IDeleteInvoiceCommand
    {
        public DeleteInvoiceCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(int id)
        {
            var expense = await _repository.Query<domain.Operation.Expense>()
                .Where(f => f.InvoiceId == id)
                .FirstOrDefaultAsync();

            if (expense != null)
            {
                _repository.DeleteSync(expense);

                var invoice = await _repository.Query<domain.Operation.ExpenseInvoice>()
                    .Where(f => f.Id == id)
                    .FirstOrDefaultAsync();

                if (invoice != null)
                {
                    _repository.DeleteSync(invoice);

                    await _repository.CommitAsync();
                }
            } else
            {
                var invoice = await _repository.Query<domain.Operation.ExpenseInvoice>()
                    .Where(f => f.Id == id)
                    .FirstOrDefaultAsync();

                if (invoice != null)
                {
                    _repository.DeleteSync(invoice);

                    await _repository.CommitAsync();
                }
            }
        }
    }
}
