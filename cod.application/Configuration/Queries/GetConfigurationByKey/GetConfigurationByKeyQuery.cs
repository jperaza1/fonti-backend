using Microsoft.EntityFrameworkCore;
using cod.application.Configuration.Queries.GetConfigurations;
using cod.application.Features.Base;
using cod.application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Configuration.Queries.GetConfigurationByKey
{
    public class GetConfigurationByKeyQuery : BaseQuery, IGetConfigurationByKeyQuery
    {
        public GetConfigurationByKeyQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<GetConfigurationsModel> Execute(string key)
        {
            var configuration = await _repository.QueryActive<domain.General.Configuration>(x => x.Key == key).FirstOrDefaultAsync();

            if (configuration == null)
            {
                return new GetConfigurationsModel
                {
                    Key = key,
                };
            }

            return new GetConfigurationsModel
            {
                Id = configuration.Id,
                IsActive = configuration.IsActive,
                Key = configuration.Key,
                Value = configuration.Value,
            };

        }
    }
}
