using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Payment.Common;
using cod.domain.Features;
using cod.domain.Operation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Payment.Commands.AddPayment
{
    public class AddPaymentCommand : BaseCommand, IAddPaymentCommand
    {
        private readonly AuthService _authService;
        public AddPaymentCommand(
            IRepository repository, 
            IExceptionFactory exceptionFactory,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task Execute(PaymentModel model)
        {
            var invoice = await _repository.WhereActive<domain.Operation.Invoice>(f => f.Id == model.InvoiceId).FirstOrDefaultAsync();

            var balance = invoice.Balance - model.Amount;

            if(balance == 0)
            {
                invoice.Paid = true;
                invoice.PaidAt = Clock.Now;
                invoice.PaidUser = _authService.User;
                invoice.PaymentStatus = domain.Features.Enums.DomainEnums.PaymentStatus.Paid;
            } else
            {
                invoice.PaymentStatus = invoice.PaymentStatus == domain.Features.Enums.DomainEnums.PaymentStatus.Pending ? domain.Features.Enums.DomainEnums.PaymentStatus.PartiallyPaid : invoice.PaymentStatus;
            }
            
            invoice.Balance = balance;
            
            _repository.UpdateSync(invoice);

            var newPayment = new domain.Operation.Payment
            {
                PaymentDate = model.PaymentDate,
                PaymentMethod = model.PaymentMethod,
                InvoiceId = model.InvoiceId,
                Amount = model.Amount,
            };

            _repository.InsertAsync(newPayment);

            await _repository.CommitAsync();
        }

        public async Task ValidateModel(PaymentModel model)
        {
            if(model.PaymentMethod.IsValid())
                this._exceptionFactory.Add("paymentMethod", this._exceptionFactory.GetMessage("Metodo de Pago", ExceptionEnums.ExceptionCode.Required));

            if (model.PaymentDate == DateTime.MinValue)
                this._exceptionFactory.Add("paymentDate", this._exceptionFactory.GetMessage("Fecha de Pago", ExceptionEnums.ExceptionCode.Required));

            if (model.Amount <= 0)
                this._exceptionFactory.Add("amount", this._exceptionFactory.GetMessage("Monto", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "PAGOS", this._exceptionFactory.Exceptions);

            var invoice = await _repository.WhereActive<domain.Operation.Invoice>(f => f.Id == model.InvoiceId).FirstOrDefaultAsync();

            var balance = invoice.Balance - model.Amount;

            if (balance < 0)
                this._exceptionFactory.Add("amount", this._exceptionFactory.GetMessage("El balance de la factura no puede ser negativo", ExceptionEnums.ExceptionCode.Default));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "PAGOS", this._exceptionFactory.Exceptions);
        }
    }
}
