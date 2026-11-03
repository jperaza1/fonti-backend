using cod.application.BankAccount.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.BankAccount.Queries.GetActiveBankAccounts
{
    public interface IGetActiveBankAccountsQuery
    {
        Task<List<BankAccountModel>> Execute();
    }
}
