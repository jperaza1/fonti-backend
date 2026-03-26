using cod.application.Expense.Common;
using System.Threading.Tasks;

namespace cod.application.Expense.Commands.UpdateExpense
{
    public interface IUpdateExpenseCommand
    {
        Task Execute(ExpenseModel model, int id);
    }
}
