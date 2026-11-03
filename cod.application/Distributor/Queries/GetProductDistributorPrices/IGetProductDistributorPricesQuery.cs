using cod.application.Distributor.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.Distributor.Queries.GetProductDistributorPrices
{
    public interface IGetProductDistributorPricesQuery
    {
        Task<List<ProductDistributorPricesModel>> Execute(int distributorId);
    }
}
