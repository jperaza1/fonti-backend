using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cod.application.Features.Base;
using cod.application.Interfaces;

namespace cod.application.Configuration.Queries.GetActivesConfigurations
{
    public class GetActivesConfigurationsQuery : BaseQuery, IGetActivesConfigurationsQuery
    {
        public GetActivesConfigurationsQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<GetActivesConfigurationsQueryModel>> Execute()
        {
            return await
                _repository
                    .QueryActive<domain.General.Configuration>()
                    .Select(config => new GetActivesConfigurationsQueryModel
                    {
                        Key = config.Key,
                        Value = config.Value
                    })
                    .ToListAsync();
        }
    }
}