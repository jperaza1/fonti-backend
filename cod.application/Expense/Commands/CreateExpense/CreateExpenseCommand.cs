using cod.application.Expense.Common;
using cod.application.ExpenseInvoice.Common;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.domain.Features;
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

        public async Task Execute(ExpenseModel model, IFormFile invoiceFile = null)
        {
            await ValidateModel(model);

            var entity = new domain.Operation.Expense
            {
                CategoryId = model.CategoryId,
                SubCategoryId = model.SubCategoryId,
                Notes = model.Notes,
                Date = model.Date,
                Total = model.Total,
                VendorId = model.VendorId,
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

                var attachementType = "EXPENSE";
                var key = entity.Id;

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
            var invoice = new domain.Operation.ExpenseInvoice
            {
                InvoiceNumber = model.ExpenseInvoice.InvoiceNumber,
                InvoiceDate = model.ExpenseInvoice.InvoiceDate,
                TaxableAmount = Math.Round(model.ExpenseInvoice.TaxableAmount, 2),
                TaxAmount = Math.Round(model.ExpenseInvoice.TaxAmount, 2),
                ExemptAmount = Math.Round(model.ExpenseInvoice.ExemptAmount, 2),
                Total = Math.Round(model.Total, 2),
                SubTotal = Math.Round(model.ExpenseInvoice.SubTotal, 2),
                Discount = Math.Round(model.ExpenseInvoice.Discount, 2),
                CreationTime = Clock.Now,
                CreatorUser = _authService.User,
                IsActive = true,
            };

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
