using cod.application.BankAccount.Common;
using cod.application.Features.Base;
using cod.application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.BankAccount.Queries.GetActiveBankAccounts
{
    public class GetActiveBankAccountsQuery : BaseQuery, IGetActiveBankAccountsQuery
    {
        public GetActiveBankAccountsQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<BankAccountModel>> Execute()
        {
            var baseQuery = await _repository.QueryActive<cod.domain.Finance.BankAccount>()
                .Select(f => new
                {
                    f.Id,
                    f.Name,
                    Bank = f.Bank.Name,
                    f.Balance,
                    f.IsActive
                }).ToListAsync();

            return baseQuery
                .Select(f => new BankAccountModel
                {
                    Id = f.Id,
                    Name = $"{f.Name} - {f.Bank}",
                    Balance = f.Balance,
                    IsActive = f.IsActive,
                }).ToList();
        }
    }
}
