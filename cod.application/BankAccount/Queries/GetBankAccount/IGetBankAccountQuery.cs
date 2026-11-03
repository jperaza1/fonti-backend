using cod.application.BankAccount.Common;
using System.Threading.Tasks;

namespace cod.application.BankAccount.Queries.GetBankAccount
{
    public interface IGetBankAccountQuery
    {
        Task<BankAccountModel> Execute(int id);
    }
}
