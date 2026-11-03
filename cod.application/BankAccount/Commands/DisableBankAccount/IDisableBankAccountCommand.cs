using System.Threading.Tasks;

namespace cod.application.BankAccount.Commands.DisableBankAccount
{
    public interface IDisableBankAccountCommand
    {
        Task Execute(int id);
    }
}
