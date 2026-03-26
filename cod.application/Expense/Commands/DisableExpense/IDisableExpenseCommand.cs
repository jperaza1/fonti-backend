using System.Threading.Tasks;

namespace cod.application.Expense.Commands.DisableExpense
{
    public interface IDisableExpenseCommand
    {
        Task Execute(int id);
    }
}
