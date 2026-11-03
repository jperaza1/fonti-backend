using cod.application.BankAccount.Common;
using cod.application.Features.Base;
using cod.application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.BankAccount.Queries.GetBankAccounts
{
    public class GetBankAccountsQuery : BaseQuery, IGetBankAccountsQuery
    {
        public GetBankAccountsQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<BankAccountModel>> Execute()
        {
            return await _repository.Query<cod.domain.Finance.BankAccount>().Select(f => new BankAccountModel
            {
                Id = f.Id,
                Name = f.Name,
                BankId = f.BankId,
                BankName = f.Bank.Name,
                Balance = f.Balance,
                IsActive = f.IsActive,
            }).ToListAsync();
        }
    }
}
