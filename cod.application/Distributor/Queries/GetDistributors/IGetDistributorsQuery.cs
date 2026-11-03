using System.Collections.Generic;
using System.Threading.Tasks;
using cod.application.Distributor.Common;

namespace cod.application.Distributor.Queries.GetDistributors
{
    public interface IGetDistributorsQuery
    {
        Task<List<DistributorModel>> Execute();
    }
}
