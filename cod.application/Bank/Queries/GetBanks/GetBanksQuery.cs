using cod.application.Bank.Common;
using cod.application.Features.Base;
using cod.application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.Bank.Queries.GetBanks
{
    public class GetBanksQuery : BaseQuery, IGetBanksQuery
    {
        public GetBanksQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<BankModel>> Execute()
        {
            return await _repository.Query<cod.domain.Data.Bank>().Select(f => new BankModel
            {
                Id = f.Id,
                Name = f.Name,
                Code = f.Code,
                IsActive = f.IsActive,
            }).OrderBy(f => f.Name).ToListAsync();
        }
    }
}
