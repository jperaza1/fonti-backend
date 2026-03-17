using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cod.application.Features.Base;
using cod.application.Interfaces;

namespace cod.application.Configuration.Queries.GetConfigurations
{
    public class GetConfigurationsQuery : BaseQuery, IGetConfigurationsQuery
    {

        public GetConfigurationsQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<GetConfigurationsModel>> Execute()
        {
            return await _repository.Query<domain.General.Configuration>()
                .Select(conf => new GetConfigurationsModel
                {
                    Id = conf.Id,
                    IsActive = conf.IsActive,
                    Key = conf.Key,
                    Value = conf.Value
                })
                .ToListAsync();
        }
    }
}
