using cod.application.Expense.Common;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace cod.application.Expense.Commands.CreateExpense
{
    public interface ICreateExpenseCommand
    {
        Task Execute(ExpenseModel model, IFormFile invoiceFile);
    }
}
