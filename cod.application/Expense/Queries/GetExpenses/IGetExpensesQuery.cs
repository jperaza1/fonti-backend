using cod.application.Expense.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.Expense.Queries.GetExpenses
{
    public interface IGetExpensesQuery
    {
        Task<List<ExpenseModel>> Execute(ExpenseFilterModel filterModel);
    }
}
