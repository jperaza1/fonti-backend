using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Expense.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Expense.Queries.GetExpense
{
    public class GetExpenseQuery : BaseQuery, IGetExpenseQuery
    {
        public GetExpenseQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<ExpenseModel> Execute(int id)
        {
            var row = await _repository.Query<domain.Operation.Expense>()
                .Where(f => f.Id == id)
                .Select(f => new ExpenseModel
                {
                    Id = f.Id,
                    CategoryId = f.CategoryId,
                    CategoryName = f.Category.Name,
                    SubCategoryId = f.SubCategoryId,
                    SubCategoryName = f.SubCateogy.Name,
                    RouteId = f.RouteId,
                    Notes = f.Notes,
                    Date = f.Date,
                    IsInvoice = f.IsInvoice,
                    Total = f.Total,
                    IsActive = f.IsActive,
                    VendorId = f.VendorId
                })
                .FirstOrDefaultAsync();

            if (row == null)
                throw new ApplicationException("Gasto no existe", "GASTO");

            return row;
        }
    }
}
