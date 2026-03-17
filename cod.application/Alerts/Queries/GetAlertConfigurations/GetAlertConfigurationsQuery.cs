using Microsoft.EntityFrameworkCore;
using cod.application.Features.Base;
using cod.application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.Alerts.Queries.GetAlertConfigurations
{
    public class GetAlertConfigurationsQuery : BaseQuery, IGetAlertConfigurationsQuery
    {
        public GetAlertConfigurationsQuery(IRepository repository) : base(repository)
        {

        }
        public async Task<List<GetAlertConfigurationsModel>> Execute()
        {
            return await _repository
                .Query<domain.Alerts.AlertConfiguration>()
                .Select(x => new GetAlertConfigurationsModel
                {
                    Id = x.Id,
                    AlertBy = x.AlertBy,
                    BodyType = x.BodyType,
                    Name = x.Name,
                    IsActive = x.IsActive
                })
                .ToListAsync();
        }
    }
}
