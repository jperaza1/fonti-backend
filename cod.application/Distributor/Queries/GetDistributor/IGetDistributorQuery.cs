using cod.application.Distributor.Common;
using System.Threading.Tasks;

namespace cod.application.Distributor.Queries.GetDistributor
{
    public interface IGetDistributorQuery
    {
        Task<DistributorModel> Execute(int id);
    }
}
