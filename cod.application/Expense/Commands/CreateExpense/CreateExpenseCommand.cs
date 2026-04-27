using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Expense.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Expense.Commands.CreateExpense
{
    public class CreateExpenseCommand : BaseCommand, ICreateExpenseCommand
    {
        public CreateExpenseCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(ExpenseModel model)
        {
            await ValidateModel(model);

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
            };

            _repository.InsertAsync(entity);

            await _repository.CommitAsync();
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
