using cod.application.Bank.Common;
using System.Threading.Tasks;

namespace cod.application.Bank.Commands.UpdateBank
{
    public interface IUpdateBankCommand
    {
        Task Execute(BankModel model, int id);
    }
}
