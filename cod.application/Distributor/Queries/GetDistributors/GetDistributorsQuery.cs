using cod.application.Distributor.Common;
using cod.application.Features.Base;
using cod.application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.Distributor.Queries.GetDistributors
{
    public class GetDistributorsQuery : BaseQuery, IGetDistributorsQuery
    {
        public GetDistributorsQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<DistributorModel>> Execute()
        {
            return await _repository.Query<domain.Data.Distributor>()
                .OrderBy(f => f.Name)
                .Select(f => new DistributorModel
                {
                    Id = f.Id,
                    Name = f.Name,
                    IsActive = f.IsActive
                }).ToListAsync();
        }
    }
}
