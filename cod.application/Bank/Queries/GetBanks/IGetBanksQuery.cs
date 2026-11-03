using cod.application.Bank.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.Bank.Queries.GetBanks
{
    public interface IGetBanksQuery
    {
        Task<List<BankModel>> Execute();
    }
}
