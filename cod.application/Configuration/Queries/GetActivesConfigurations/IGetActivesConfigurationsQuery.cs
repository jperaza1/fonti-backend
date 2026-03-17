using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.Configuration.Queries.GetActivesConfigurations
{
    public interface IGetActivesConfigurationsQuery
    {
        Task<List<GetActivesConfigurationsQueryModel>> Execute();
    }
}
