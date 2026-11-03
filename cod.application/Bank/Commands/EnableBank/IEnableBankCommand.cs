using System.Threading.Tasks;

namespace cod.application.Bank.Commands.EnableBank
{
    public interface IEnableBankCommand
    {
        Task Execute(int id);
    }
}
