using cod.application.Sale.Commands.ConsolidateSale;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.Sale.Queries.GetConsolidateAvailability
{
    public interface IGetConsolidateAvailabilityQuery
    {
        Task<List<ConsolidateAvailabilityLine>> Execute(ConsolidateSaleModel model);
    }
}
