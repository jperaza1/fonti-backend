using cod.application.Bank.Common;
using cod.application.Features.Base;
using cod.application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Bank.Queries.GetBank
{
    public class GetBankQuery : BaseQuery, IGetBankQuery
    {
        public GetBankQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<BankModel> Execute(int id)
        {
            var bank = await _repository.Query<cod.domain.Data.Bank>()
                .Where(f => f.Id == id)
                .Select(f => new BankModel
                {
                    Id = f.Id,
                    Name = f.Name,
                    Code = f.Code,
                    Alias = f.Alias,
                    IsActive = f.IsActive,
                }).FirstOrDefaultAsync();

            if (bank == null)
                throw new ApplicationException("Banco no existe", "BANCO");

            return bank;
        }
    }
}
