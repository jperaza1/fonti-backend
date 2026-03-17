using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.Configuration.Queries.GetConfigurations
{
    public interface IGetConfigurationsQuery
    {
        Task<List<GetConfigurationsModel>> Execute();
    }
}