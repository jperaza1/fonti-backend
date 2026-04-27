using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Invoice.Common;
using cod.domain.Data;
using cod.domain.Features;
using cod.domain.Operation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            await this.ValidateModel(model);

            DateTime? dueDate = null;
            var balance = Math.Round(model.Total, 2);
            var vendor = new domain.Data.Vendor();


            var invoice = new domain.Operation.Invoice
            {
                PurchaseOrderId = model.PurchaseOrderId,
                InvoiceNumber = model.InvoiceNumber,
                Description = model.Description,
                InvoiceDate = model.InvoiceDate,
                InvoiceType = InvoiceType.Expense,
                PaymentType = model.PaymentType,
                PaymentStatus = model.PaymentType == PaymentType.Cash ? PaymentStatus.Paid : PaymentStatus.Pending,
                TaxableAmount = Math.Round(model.TaxableAmount, 2),

                TaxAmount = Math.Round(model.TaxAmount, 2),
                ExemptAmount = Math.Round(model.ExemptAmount, 2),
                Total = Math.Round(model.Total, 2),
                SubTotal = Math.Round(model.SubTotal, 2),
                Discount = Math.Round(model.Discount, 2),
                CategoryId = model.CategoryId
            };

            if (model.PurchaseOrderId.HasValue)
            {
                var purchaseOrder = await _repository.AllActive<domain.Operation.PurchaseOrder>()
                    .Where(f => f.Id == model.PurchaseOrderId).FirstOrDefaultAsync();

                if (purchaseOrder != null)
                {
                    invoice.CafeteriaId = purchaseOrder.CafeteriaId;
                    invoice.VendorId = purchaseOrder.VendorId;
                    vendor = purchaseOrder.Vendor;
                }

            }
            else
            {
                vendor = await _repository.Query<domain.Data.Vendor>()
                        .Where(f => f.Id == model.VendorId).FirstOrDefaultAsync();

                if (vendor != null)
                {
                    invoice.CafeteriaId = model.CafeteriaId;
                    invoice.VendorId = model.VendorId;
                }
            }

            /*if (model.PaymentType == PaymentType.Credit)
            {
                if (!vendor.HasCredit)
                    throw new ApplicationException("Este proveedor no otorga crédito. No se puede adjuntar una factura al credito", "CANCELED");

                dueDate = model.InvoiceDate.AddDays(vendor.CreditDays);
            }
            else
            {
                balance = 0.00;

                var baseQuery = _repository.QueryActive<domain.Data.Type>()
                    .Where(f => f.TypeGroup.Name == "EXPENSE_INVOICE_CATEGORY" && f.Name == "Contado");

                var type = await baseQuery.FirstOrDefaultAsync();

                var payment = new domain.Operation.Payment
                {
                    PaymentDate = Clock.Now,
                    Amount = Math.Round(model.Total, 2),
                    PaymentMethod = model.PaymentMethod,
                    IsActive = true,
                    CreationTime = Clock.Now,
                    CreatorUser = _authService.User
                };
                invoice.Payments = new List<domain.Operation.Payment> { payment };
                invoice.CategoryId = type.Id;
            }*/

            /*bool duplicateName = await _repository
                .AnyAsync<domain.Operation.Invoice>(f => f.InvoiceNumber.ToUpper() == model.InvoiceNumber.ToUpper() && f.VendorId == invoice.VendorId);
            if (model.HasInvoice && duplicateName)
                this._exceptionFactory.Add("invoiceNumber", this._exceptionFactory.GetMessage("Numero de Factura", ExceptionEnums.ExceptionCode.Duplicate));*/

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "FACTURAS", this._exceptionFactory.Exceptions);

            invoice.Balance = balance;
            invoice.DueDate = dueDate;

            _repository.InsertAsync(invoice);

            await _repository.CommitAsync();
        }

        public async Task ValidateModel(InvoiceModel model)
        {
            if (model.HasInvoice && string.IsNullOrEmpty(model.InvoiceNumber) )
                this._exceptionFactory.Add("invoiceNumber", this._exceptionFactory.GetMessage("Numero de Factura", ExceptionEnums.ExceptionCode.Required));

            if (!model.PaymentType.IsValid())
                this._exceptionFactory.Add("paymentType", this._exceptionFactory.GetMessage("Tipo de Pago", ExceptionEnums.ExceptionCode.Required));
            
            if(model.PaymentType == PaymentType.Credit && model.CategoryId <= 0)
                this._exceptionFactory.Add("category", this._exceptionFactory.GetMessage("Categoria", ExceptionEnums.ExceptionCode.Required));

            if (model.PaymentType == PaymentType.Cash && !model.PaymentMethod.IsValid())
                this._exceptionFactory.Add("paymentMethod", this._exceptionFactory.GetMessage("Metodo de Pago", ExceptionEnums.ExceptionCode.Required));

            if (model.InvoiceDate == DateTime.MinValue)
                this._exceptionFactory.Add("invoiceDate", this._exceptionFactory.GetMessage("Fecha de Factura", ExceptionEnums.ExceptionCode.Required));

            if (!model.PurchaseOrderId.HasValue && model.CafeteriaId <= 0)
                this._exceptionFactory.Add("cafeteria", this._exceptionFactory.GetMessage("Cafeteria", ExceptionEnums.ExceptionCode.Required));

            if (!model.PurchaseOrderId.HasValue && model.VendorId <= 0)
                this._exceptionFactory.Add("vendor", this._exceptionFactory.GetMessage("Proveedor", ExceptionEnums.ExceptionCode.Required));

            if (model.HasInvoice && model.TaxableAmount <= 0 && model.ExemptAmount <= 0)
                this._exceptionFactory.Add("taxableAmount", this._exceptionFactory.GetMessage("Monto Gravado", ExceptionEnums.ExceptionCode.Required));

            if (model.HasInvoice && model.TaxableAmount <= 0 && model.ExemptAmount <= 0)
                this._exceptionFactory.Add("exemptAmount", this._exceptionFactory.GetMessage("Monto Exento", ExceptionEnums.ExceptionCode.Required));

            if (model.HasInvoice && model.TaxableAmount > 0 && model.TaxAmount <= 0)
                this._exceptionFactory.Add("taxAmount", this._exceptionFactory.GetMessage("ISV 15%", ExceptionEnums.ExceptionCode.Required));

            if (model.InvoiceDate == DateTime.MinValue)
                this._exceptionFactory.Add("invoiceDate", this._exceptionFactory.GetMessage("Fecha de Factura", ExceptionEnums.ExceptionCode.Required));

            if (model.HasInvoice && model.SubTotal <= 0)
                this._exceptionFactory.Add("subTotal", this._exceptionFactory.GetMessage("SubTotal", ExceptionEnums.ExceptionCode.Required));

            if (model.Total <= 0)
                this._exceptionFactory.Add("total", this._exceptionFactory.GetMessage("Total", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "FACTURAS", this._exceptionFactory.Exceptions);


        }

    }
}
