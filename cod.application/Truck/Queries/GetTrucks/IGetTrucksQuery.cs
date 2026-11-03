using cod.application.Truck.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.Truck.Queries.GetTrucks
{
    public interface IGetTrucksQuery
    {
        Task<List<TruckModel>> Execute();
    }
}
