using cod.application.BankAccount.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.BankAccount.Queries.GetBankAccounts
{
    public interface IGetBankAccountsQuery
    {
        Task<List<BankAccountModel>> Execute();
    }
}
