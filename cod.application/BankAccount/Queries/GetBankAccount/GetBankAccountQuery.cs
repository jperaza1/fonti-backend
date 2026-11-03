using cod.application.BankAccount.Common;
using cod.application.Features.Base;
using cod.application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.BankAccount.Queries.GetBankAccount
{
    public class GetBankAccountQuery : BaseQuery, IGetBankAccountQuery
    {
        public GetBankAccountQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<BankAccountModel> Execute(int id)
        {
            var account = await _repository.Query<cod.domain.Finance.BankAccount>()
                .Where(f => f.Id == id)
                .Select(f => new BankAccountModel
                {
                    Id = f.Id,
                    Name = f.Name,
                    Balance = f.Balance,
                    BankId = f.BankId,
                    BankName = f.Bank.Name,
                    IsActive = f.IsActive,
                }).FirstOrDefaultAsync();

            if (account == null)
                throw new ApplicationException("Cuenta bancaria no existe", "CUENTA_BANCARIA");

            return account;
        }
    }
}
