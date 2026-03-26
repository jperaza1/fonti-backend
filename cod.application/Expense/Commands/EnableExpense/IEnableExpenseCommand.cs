using System.Threading.Tasks;

namespace cod.application.Expense.Commands.EnableExpense
{
    public interface IEnableExpenseCommand
    {
        Task Execute(int id);
    }
}
