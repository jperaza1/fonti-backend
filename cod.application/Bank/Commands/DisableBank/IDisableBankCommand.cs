using System.Threading.Tasks;

namespace cod.application.Bank.Commands.DisableBank
{
    public interface IDisableBankCommand
    {
        Task Execute(int id);
    }
}
