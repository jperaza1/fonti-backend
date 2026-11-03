using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.domain.Data;
using cod.domain.Features;
using cod.domain.Operation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Invoice.Commands.ChangePaymentType
{
    public class ChangePaymentTypeCommand : BaseCommand, IChangePaymentTypeCommand
    {
        private readonly AuthService _authService;
        public ChangePaymentTypeCommand(
            IRepository repository,
            AuthService authService,
            IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task Execute(int id)
        {
            DateTime? dueDate = null;

            var invoice = await _repository.AllActive<domain.Operation.ExpenseInvoice>
                ().FirstOrDefaultAsync(f => f.Id == id);

            if (invoice != null)
            {
                if(invoice.PaymentType == domain.Features.Enums.DomainEnums.PaymentType.Cash)
                {

                    dueDate = invoice.InvoiceDate.AddDays(30);
                    invoice.DueDate = dueDate;
                    invoice.Balance = invoice.Total;
                    invoice.PaymentStatus = domain.Features.Enums.DomainEnums.PaymentStatus.Pending;
                    invoice.Paid = false;
                    invoice.PaidAt = null;
                    invoice.PaidUser = null;
                    invoice.PaymentType = domain.Features.Enums.DomainEnums.PaymentType.Credit;

                    foreach(var item in invoice.Payments)
                    {
                        _repository.DeleteSync<InvoicePayment>(item);
                    }

                } else
                {
                    var payment = new InvoicePayment
                    {
                        PaymentDate = Clock.Now,
                        Amount = Math.Round(invoice.Total, 2),
                        PaymentMethod = domain.Features.Enums.DomainEnums.PaymentMethod.Cash,
                        IsActive = true,
                        CreationTime = Clock.Now,
                        CreatorUser = _authService.User
                    };
                    invoice.Balance = 0.00;
                    invoice.Payments = new List<domain.Operation.InvoicePayment> { payment };
                    invoice.Paid = true;
                    invoice.PaidAt = Clock.Now;
                    invoice.PaidUser = _authService.User;
                    invoice.PaymentType = domain.Features.Enums.DomainEnums.PaymentType.Cash;
                    invoice.PaymentStatus = domain.Features.Enums.DomainEnums.PaymentStatus.Paid;
                }

                _repository.InsertAsync(new domain.General.DataChangeLog
                {
                    EntityId = invoice.Id,
                    Entity = "INVOICE",
                    Date = Clock.Now,
                    Log = $"Cambio en el Tipo de Pago ({invoice.PaymentType.ToString()})"
                });

                _repository.UpdateSync(invoice);
                await _repository.CommitAsync();
            }

        }
    }
}
