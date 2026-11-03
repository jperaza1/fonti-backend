using cod.application.BankAccount.Common;
using System.Threading.Tasks;

namespace cod.application.BankAccount.Commands.CreateBankAccount
{
    public interface ICreateBankAccountCommand
    {
        Task Execute(BankAccountModel model);
    }
}
