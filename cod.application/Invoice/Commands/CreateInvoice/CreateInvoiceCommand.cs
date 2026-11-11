using cod.application.Common.Clases;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Invoice.Common;
using cod.domain.Common;
using cod.domain.Features;
using cod.domain.Operation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Invoice.Commands.CreateInvoice
{
    public class CreateInvoiceCommand : BaseCommand, ICreateInvoiceCommand
    {
        private readonly AuthService _authService;

        public CreateInvoiceCommand(
            IRepository repository,
            IExceptionFactory exceptionFactory,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task Execute(InvoiceModel model)
        {
            var timeZone = _authService.ClientTimezone;
            model.InvoiceDate = DateService.GetCLientLocalTime(model.InvoiceDate, timeZone);
            await ValidateModel(model);

            var purchaseOrder = await _repository.Query<domain.Operation.PurchaseOrder>()
                .Where(f => f.Id == model.PurchaseOrderId)
                .FirstOrDefaultAsync();

            if (purchaseOrder == null || !purchaseOrder.IsActive)
                throw new ApplicationException("CANCELED", "La orden de compra no existe", this._exceptionFactory.Exceptions);

            if (purchaseOrder.OrderStatus != PurchaseOrderStatus.Processing)
                throw new ApplicationException("CANCELED", "Solo se agregan facturas con la orden en proceso", this._exceptionFactory.Exceptions);

            var duplicate = await _repository.Query<domain.Operation.ExpenseInvoice>()
                .AnyAsync(f => f.InvoiceNumber == model.InvoiceNumber
                    && f.PurchaseOrderId != null
                    && f.PurchaseOrder.VendorId == purchaseOrder.VendorId);

            if (duplicate)
            {
                this._exceptionFactory.Add("invoiceNumber", this._exceptionFactory.GetMessage("Numero de Factura", ExceptionEnums.ExceptionCode.Duplicate));
                throw new ApplicationException("CANCELED", "FACTURAS", this._exceptionFactory.Exceptions);
            }

            var isCash = model.PaymentType == PaymentType.Cash;
            var total = Math.Round(model.Total, 2);
            var invoice = new domain.Operation.ExpenseInvoice
            {
                PurchaseOrderId = purchaseOrder.Id,
                InvoiceNumber = model.InvoiceNumber,
                Description = model.Description,
                InvoiceDate = model.InvoiceDate,
                PaymentType = model.PaymentType,
                PaymentStatus = isCash ? PaymentStatus.Paid : PaymentStatus.Pending,
                TaxableAmount = Math.Round(model.TaxableAmount, 2),
                ExemptAmount = Math.Round(model.ExemptAmount, 2),
                Discount = Math.Round(model.Discount, 2),
                SubTotal = Math.Round(model.SubTotal, 2),
                TaxAmount = Math.Round(model.TaxAmount, 2),
                Total = total,
                Balance = isCash ? 0 : total,
                DueDate = isCash ? (DateTime?)null : model.InvoiceDate.AddDays(30),
                Paid = isCash,
                PaidAt = isCash ? Clock.Now : (DateTime?)null,
                PaidUser = isCash ? _authService.User : null
            };

            if (isCash)
            {
                invoice.Payments = new List<InvoicePayment>
                {
                    new InvoicePayment
                    {
                        PaymentDate = Clock.Now,
                        Amount = total,
                        PaymentMethod = model.PaymentMethod,
                        IsActive = true,
                        CreationTime = Clock.Now,
                        CreatorUser = _authService.User
                    }
                };
            }

            _repository.InsertAsync(invoice);
            await _repository.CommitAsync();
        }

        private async Task ValidateModel(InvoiceModel model)
        {
            if (!model.PurchaseOrderId.HasValue || model.PurchaseOrderId.Value <= 0)
                this._exceptionFactory.Add("purchaseOrder", this._exceptionFactory.GetMessage("Orden de Compra", ExceptionEnums.ExceptionCode.Required));

            if (string.IsNullOrWhiteSpace(model.InvoiceNumber))
                this._exceptionFactory.Add("invoiceNumber", this._exceptionFactory.GetMessage("Numero de Factura", ExceptionEnums.ExceptionCode.Required));

            if (!model.PaymentType.IsValid())
                this._exceptionFactory.Add("paymentType", this._exceptionFactory.GetMessage("Tipo de Pago", ExceptionEnums.ExceptionCode.Required));

            if (model.PaymentType == PaymentType.Cash && !model.PaymentMethod.IsValid())
                this._exceptionFactory.Add("paymentMethod", this._exceptionFactory.GetMessage("Metodo de Pago", ExceptionEnums.ExceptionCode.Required));

            if (model.InvoiceDate == DateTime.MinValue)
                this._exceptionFactory.Add("invoiceDate", this._exceptionFactory.GetMessage("Fecha de Factura", ExceptionEnums.ExceptionCode.Required));

            if (model.TaxableAmount <= 0 && model.ExemptAmount <= 0)
                this._exceptionFactory.Add("taxableAmount", this._exceptionFactory.GetMessage("Monto Gravado", ExceptionEnums.ExceptionCode.Required));

            if (model.SubTotal <= 0)
                this._exceptionFactory.Add("subTotal", this._exceptionFactory.GetMessage("SubTotal", ExceptionEnums.ExceptionCode.Required));

            if (model.Total <= 0)
                this._exceptionFactory.Add("total", this._exceptionFactory.GetMessage("Total", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "FACTURAS", this._exceptionFactory.Exceptions);

            await Task.CompletedTask;
        }
    }
}
