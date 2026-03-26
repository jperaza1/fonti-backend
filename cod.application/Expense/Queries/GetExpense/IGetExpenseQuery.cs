using cod.application.Expense.Common;
using System.Threading.Tasks;

namespace cod.application.Expense.Queries.GetExpense
{
    public interface IGetExpenseQuery
    {
        Task<ExpenseModel> Execute(int id);
    }
}
