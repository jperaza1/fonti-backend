using System.Threading.Tasks;

namespace cod.application.BankAccount.Commands.EnableBankAccount
{
    public interface IEnableBankAccountCommand
    {
        Task Execute(int id);
    }
}
