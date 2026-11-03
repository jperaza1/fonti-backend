using cod.application.Bank.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.Bank.Queries.GetActiveBanks
{
    public interface IGetActiveBanksQuery
    {
        Task<List<BankModel>> Execute();
    }
}
