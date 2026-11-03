using cod.application.Truck.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.Truck.Queries.GetActiveTrucks
{
    public interface IGetActiveTrucksQuery
    {
        Task<List<TruckModel>> Execute();
    }
}
