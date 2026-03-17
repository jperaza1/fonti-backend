using cod.application.Configuration.Queries.GetConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Configuration.Queries.GetConfigurationByKey
{
    public interface IGetConfigurationByKeyQuery
    {
        Task<GetConfigurationsModel> Execute(string key);
    }
}
