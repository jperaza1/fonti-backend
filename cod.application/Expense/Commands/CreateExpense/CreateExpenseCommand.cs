using cod.application.Common.Clases;
using cod.application.Expense.Common;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.domain.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Expense.Commands.CreateExpense
{
    public class CreateExpenseCommand : BaseCommand, ICreateExpenseCommand
    {
        private readonly AuthService _authService;
        private readonly IConfigurationService _configurationService;
        public CreateExpenseCommand(
            IRepository repository, 
            IExceptionFactory exceptionFactory,
            AuthService authService,
            IConfigurationService configurationService) : base(repository, exceptionFactory)
        {
            _authService = authService;
            _configurationService = configurationService;
        }

        public async Task Execute(ExpenseModel model, IFormFile invoiceFile)
        {
            await ValidateModel(model);

            if (invoiceFile == null)
                throw new ApplicationException("SE REQUIERE UN ARCHIVO", "GASTO / FACTURA");

            var timezone = _authService.ClientTimezone;

            model.Date = DateService.GetCLientLocalTime(model.Date, timezone);
            model.Date = model.Date.Date.AddHours(12);

            var entity = new domain.Operation.Expense
            {
                CategoryId = model.CategoryId,
                SubCategoryId = model.SubCategoryId,
                Notes = model.Notes,
                Date = model.Date,
                IsInvoice = model.IsInvoice,
                Total = model.Total,
                VendorId = model.VendorId,
                RouteId = model.RouteId,
                TruckId = model.TruckId,
            };

            if (model.ExpenseInvoice != null)
            {
                entity.Invoice = CreateInvoice(model, _authService);
            }

            _repository.InsertAsync(entity);

            await _repository.CommitAsync();

            var documentBaseUrl = await _configurationService.Get<string>("BASE_URL_DOCUMENT");

            if (invoiceFile != null)
            {
                using var httpClient = new HttpClient();

                var attachementType = model.ExpenseInvoice != null ? "INVOICE" : "EXPENSE";
                var key = model.ExpenseInvoice != null ? entity.InvoiceId : entity.Id;

                var url = $"{documentBaseUrl}Attachment/Upload" +
                          $"?attachmentType={attachementType}" +
                          $"&tenantId={_authService.TenantId}" +
                          $"&key={key}" +
                          $"&entity={attachementType}" +
                          $"&user={_authService.User}";

                using var content = new MultipartFormDataContent();

                using var stream = invoiceFile.OpenReadStream();
                using var fileContent = new StreamContent(stream);

                fileContent.Headers.ContentType = new MediaTypeHeaderValue(invoiceFile.ContentType ?? "application/octet-stream");

                content.Add(fileContent, "file", invoiceFile.FileName);

                var response = await httpClient.PostAsync(url, content);
            }
        }

        public static domain.Operation.ExpenseInvoice CreateInvoice(ExpenseModel model, AuthService _authService)
        {
            DateTime? dueDate = null;
            var balance = Math.Round(model.Total, 2);

            var invoice = new domain.Operation.ExpenseInvoice
            {
                InvoiceNumber = model.ExpenseInvoice.InvoiceNumber,
                InvoiceDate = model.Date,
                TaxableAmount = Math.Round(model.ExpenseInvoice.TaxableAmount, 2),
                TaxAmount = Math.Round(model.ExpenseInvoice.TaxAmount, 2),
                ExemptAmount = Math.Round(model.ExpenseInvoice.ExemptAmount, 2),
                Total = Math.Round(model.Total, 2),
                SubTotal = Math.Round(model.ExpenseInvoice.SubTotal, 2),
                Discount = Math.Round(model.ExpenseInvoice.Discount, 2),
                CreationTime = Clock.Now,
                CreatorUser = _authService.User,
                IsActive = true,
                PaymentStatus = model.PaymentType == PaymentType.Cash ? PaymentStatus.Paid : PaymentStatus.Pending,
                PaymentType = model.PaymentType,
                IsRefund = model.IsRefund,
                Balance = balance,
            };

            if(model.PaymentType == PaymentType.Cash)
            {
                balance = 0.00;
                invoice.Paid = true;
                invoice.PaidAt = Clock.Now;
                invoice.PaidUser = _authService.User;
                invoice.PaymentStatus = PaymentStatus.Paid;
            } else
            {
                dueDate = model.Date.AddDays(30);
                invoice.DueDate = dueDate;
            }
            
            return invoice;
        }

        public async Task ValidateModel(ExpenseModel model)
        {
            if (model.CategoryId <= 0)
                this._exceptionFactory.Add("routeId", this._exceptionFactory.GetMessage("Ruta", ExceptionEnums.ExceptionCode.Required));

            if (model.CategoryId <= 0)
                this._exceptionFactory.Add("categoryId", this._exceptionFactory.GetMessage("Categoría", ExceptionEnums.ExceptionCode.Required));

            if (model.SubCategoryId <= 0)
                this._exceptionFactory.Add("subCategoryId", this._exceptionFactory.GetMessage("Subcategoría", ExceptionEnums.ExceptionCode.Required));

            if (model.Total < 0)
                this._exceptionFactory.Add("total", "El total no puede ser negativo.");

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "GASTO", this._exceptionFactory.Exceptions);

            var categoryExists = await _repository.AnyAsync<domain.Data.Category>(c => c.Id == model.CategoryId);
            if (!categoryExists)
                this._exceptionFactory.Add("categoryId", "La categoría no existe.");

            var sub = await _repository.Query<domain.Data.SubCategory>()
                .Where(s => s.Id == model.SubCategoryId)
                .Select(s => new { s.Id, s.CategoryId })
                .FirstOrDefaultAsync();

            if (sub == null)
                this._exceptionFactory.Add("subCategoryId", "La subcategoría no existe.");
            else if (sub.CategoryId != model.CategoryId)
                this._exceptionFactory.Add("subCategoryId", "La subcategoría no pertenece a la categoría indicada.");

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "GASTO", this._exceptionFactory.Exceptions);
        }
    }
}
