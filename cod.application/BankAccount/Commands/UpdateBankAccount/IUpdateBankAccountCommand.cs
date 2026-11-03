using cod.application.BankAccount.Common;
using System.Threading.Tasks;

namespace cod.application.BankAccount.Commands.UpdateBankAccount
{
    public interface IUpdateBankAccountCommand
    {
        Task Execute(BankAccountModel model, int id);
    }
}
