using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Payment.Common;
using cod.domain.Features;
using cod.domain.Features.Enums;
using cod.domain.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Payment.Commands.CreatePayment
{
    public class CreatePaymentCommand : BaseCommand, ICreatePaymentCommand
    {
        private readonly AuthService _authService;
        private readonly IConfigurationService _configurationService;
        public CreatePaymentCommand(
            IRepository repository, 
            IExceptionFactory exceptionFactory,
            AuthService authService,
            IConfigurationService configurationService) : base(repository, exceptionFactory)
        {
            _authService = authService;
            _configurationService = configurationService;
        }

        public async Task<int> Execute(PaymentModel model)
        {
            ValidModel(model);

            var correlative = await _configurationService.Get<int>("CORRELATIVE_PAYMENT");

            correlative++;

            var payment = new domain.Finance.Payment
            {
                Reference = $"PA{correlative:D6}",
                InvoiceCount = model.InvoiceCount,
                PaymentDate = model.PaymentDate,
                PaymentStatus = PaymentStatus.Created,
                Notes = model.Notes,
                BankAccountId = model.BankAccountId,
                Details = GenerateDetails(model, _authService)
            };

            payment.Amount = Math.Round(model.Details.Sum(f => f.Amount), 2);

            await _configurationService.UpdateConfiguration("CORRELATIVE_PAYMENT", $"{correlative}");

            _repository.InsertAsync(payment);
            await _repository.CommitAsync();

            foreach (var item in model.Details)
            {
                await _repository.UpdateAsync<domain.Operation.ExpenseInvoice>(
                f => f.Id == item.InvoiceId,
                f => new domain.Operation.ExpenseInvoice
                {
                    PaymentId = payment.Id,
                });
            }

            await _repository.CommitAsync();
            return payment.Id;
        }

        public List<PaymentDetail> GenerateDetails(PaymentModel model, AuthService authService)
        {
            return model.Details.Select(g => new PaymentDetail
            {
                InvoiceId = g.InvoiceId,
                Amount = Math.Round(g.Amount, 2),
                IsActive = true,
                CreationTime = Clock.Now,
                CreatorUser = authService.User,
                Description = string.Empty
            }).ToList();
        }

        public void ValidModel(PaymentModel model) 
        {
            if (model.BankAccountId <= 0)
                this._exceptionFactory.Add("bankAccount", this._exceptionFactory.GetMessage("Cuenta de Banco", ExceptionEnums.ExceptionCode.Required));

            if (model.PaymentDate == DateTime.MinValue)
                this._exceptionFactory.Add("paymentDate", this._exceptionFactory.GetMessage("Fecha del Pago", ExceptionEnums.ExceptionCode.Required));

            if(model.InvoiceCount <= 0)
                this._exceptionFactory.Add("invoiceCount", this._exceptionFactory.GetMessage("Facturas", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "FACTURAS", this._exceptionFactory.Exceptions);
        }
    }
}
