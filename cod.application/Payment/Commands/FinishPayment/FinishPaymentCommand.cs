using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Payment.Commands.SendEmailNotification;
using cod.application.Payment.Common;
using cod.domain.Features;
using cod.domain.Finance;
using cod.domain.Operation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Payment.Commands.FinishPayment
{
    public class FinishPaymentCommand : BaseCommand, IFinishPaymentCommand
    {
        private readonly AuthService _authService;
        private readonly ISendEmailNotificationCommand _sendEmailNotificationCommand;
        public FinishPaymentCommand(
            IRepository repository, 
            IExceptionFactory exceptionFactory,
            ISendEmailNotificationCommand sendEmailNotificationCommand,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
            _sendEmailNotificationCommand = sendEmailNotificationCommand;
        }

        public async Task Execute(int id, PaymentModel model)
        {
            if(string.IsNullOrEmpty(model.BankReference))
                throw new ApplicationException("Se Requiere una Referncia Bancaria", "FACTURAS", this._exceptionFactory.Exceptions);

            var payment = await _repository.AllAsync<domain.Finance.Payment>()
                .Where(f => f.Id == id).FirstOrDefaultAsync();

            if (payment != null)
            {
                payment.PaymentStatus = domain.Features.Enums.PaymentStatus.Closed;
                payment.BankReference = model.BankReference;

                var bankAccount = await _repository.Query<domain.Finance.BankAccount>()
                    .FirstOrDefaultAsync(g => g.Id == payment.BankAccountId);

                if (bankAccount != null)
                {
                    bankAccount.Balance -= payment.Amount;

                    _repository.UpdateSync(bankAccount);
                }

                foreach (var item in payment.Details)
                {
                    var invoice = item.Invoice;

                    var balance = invoice.Balance;

                    invoice.Paid = true;
                    invoice.PaidAt = Clock.Now;
                    invoice.PaidUser = _authService.User;
                    invoice.Balance = invoice.Balance - item.Amount;
                    invoice.PaymentStatus = domain.Features.Enums.DomainEnums.PaymentStatus.Paid;

                    _repository.UpdateSync(invoice);

                    var invoicePayment = new InvoicePayment
                    {
                        InvoiceId = invoice.Id,
                        PaymentDate = Clock.Now,
                        PaymentMethod = domain.Features.Enums.DomainEnums.PaymentMethod.Transfer,
                        Amount = balance,
                    };

                    _repository.InsertAsync(invoicePayment);
                }

                _repository.UpdateSync(payment);

                await _repository.CommitAsync();

                await _sendEmailNotificationCommand.Execute(id);
            }
        }
    }
}
