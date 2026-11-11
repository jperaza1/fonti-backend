using cod.application.Common.Clases;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Invoice.Common;
using cod.domain.Common;
using cod.domain.Features;
using cod.domain.Operation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Invoice.Commands.CreateInvoice
{
    public class CreateInvoiceCommand : BaseCommand, ICreateInvoiceCommand
    {
        private readonly AuthService _authService;
        private readonly IConfigurationService _configurationService;

        public CreateInvoiceCommand(
            IRepository repository,
            IExceptionFactory exceptionFactory,
            AuthService authService,
            IConfigurationService configurationService) : base(repository, exceptionFactory)
        {
            _authService = authService;
            _configurationService = configurationService;
        }

        public async Task Execute(InvoiceModel model, IFormFile invoiceFile)
        {
            var timeZone = _authService.ClientTimezone;
            model.InvoiceDate = DateService.GetCLientLocalTime(model.InvoiceDate, timeZone);
            await ValidateModel(model, invoiceFile);

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

            var plantRouteId = await _repository.QueryActive<cod.domain.Data.Route>()
                .Where(route => route.Name.ToLower() == "planta")
                .Select(route => route.Id)
                .FirstOrDefaultAsync();

            if (plantRouteId <= 0)
                throw new ApplicationException("CANCELED", "No existe la ruta Planta", this._exceptionFactory.Exceptions);

            var categoryId = await _repository.QueryActive<cod.domain.Data.Category>()
                .Where(category => category.Name.ToLower() == "produccion")
                .Select(category => category.Id)
                .FirstOrDefaultAsync();

            var subCategoryId = categoryId <= 0
                ? 0
                : await _repository.QueryActive<cod.domain.Data.SubCategory>()
                    .Where(subCategory => subCategory.CategoryId == categoryId
                        && subCategory.Name.ToLower() == "compra de suministros")
                    .Select(subCategory => subCategory.Id)
                    .FirstOrDefaultAsync();

            if (categoryId <= 0 || subCategoryId <= 0)
                throw new ApplicationException("CANCELED", "No existe la categoría Producción / Compra de suministros", this._exceptionFactory.Exceptions);

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

            var expense = new domain.Operation.Expense
            {
                Invoice = invoice,
                RouteId = plantRouteId,
                VendorId = purchaseOrder.VendorId,
                CategoryId = categoryId,
                SubCategoryId = subCategoryId,
                Date = model.InvoiceDate.Date.AddHours(12),
                Total = total,
                IsInvoice = true,
                Notes = model.Description
            };

            _repository.InsertAsync(invoice);
            _repository.InsertAsync(expense);
            await _repository.CommitAsync();

            var documentBaseUrl = await _configurationService.Get<string>("BASE_URL_DOCUMENT");
            using var httpClient = new HttpClient();
            var url = $"{documentBaseUrl}Attachment/Upload" +
                      $"?attachmentType=INVOICE" +
                      $"&tenantId={_authService.TenantId}" +
                      $"&key={invoice.Id}" +
                      $"&entity=INVOICE" +
                      $"&user={_authService.User}";

            using var content = new MultipartFormDataContent();
            using var stream = invoiceFile.OpenReadStream();
            using var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(invoiceFile.ContentType ?? "application/octet-stream");
            content.Add(fileContent, "file", invoiceFile.FileName);
            await httpClient.PostAsync(url, content);
        }

        private async Task ValidateModel(InvoiceModel model, IFormFile invoiceFile)
        {
            if (!model.PurchaseOrderId.HasValue || model.PurchaseOrderId.Value <= 0)
                this._exceptionFactory.Add("purchaseOrder", this._exceptionFactory.GetMessage("Orden de Compra", ExceptionEnums.ExceptionCode.Required));

            if (invoiceFile == null || invoiceFile.Length == 0)
                this._exceptionFactory.Add("invoiceFile", this._exceptionFactory.GetMessage("Archivo", ExceptionEnums.ExceptionCode.Required));

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
