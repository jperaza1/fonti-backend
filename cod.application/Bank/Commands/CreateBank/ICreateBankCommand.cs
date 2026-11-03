using cod.application.Bank.Common;
using System.Threading.Tasks;

namespace cod.application.Bank.Commands.CreateBank
{
    public interface ICreateBankCommand
    {
        Task Execute(BankModel model);
    }
}
