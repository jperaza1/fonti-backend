using cod.application.Bank.Common;
using System.Threading.Tasks;

namespace cod.application.Bank.Queries.GetBank
{
    public interface IGetBankQuery
    {
        Task<BankModel> Execute(int id);
    }
}
