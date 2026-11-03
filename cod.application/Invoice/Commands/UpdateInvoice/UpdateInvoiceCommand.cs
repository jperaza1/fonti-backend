using cod.application.Common.Clases;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Invoice.Common;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Invoice.Commands.UpdateInvoice
{
    public class UpdateInvoiceCommand : BaseCommand, IUpdateInvoiceCommand
    {
        private readonly AuthService _authService;
        public UpdateInvoiceCommand(
            IRepository repository,
            IExceptionFactory exceptionFactory,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task Execute(int id, InvoiceModel model)
        {
            var invoice = await _repository.Query<domain.Operation.ExpenseInvoice>()
                .Where(f => f.Id == id)
                .FirstOrDefaultAsync();

            var expense = await _repository.Query<domain.Operation.Expense>()
                .Where(f => f.InvoiceId == id)
                .FirstOrDefaultAsync();

            var timezone = _authService.ClientTimezone;

            model.InvoiceDate = DateService.GetCLientLocalTime(model.InvoiceDate, timezone);
            model.InvoiceDate = model.InvoiceDate.Date.AddHours(12);

            if (expense != null)
            {
                expense.Date = model.InvoiceDate;
                _repository.UpdateSync(expense);
            }

            invoice.InvoiceDate = model.InvoiceDate;
            invoice.IsRefund = model.IsRefund;
            _repository.UpdateSync(invoice);

            await _repository.CommitAsync();
        }
    }
}
