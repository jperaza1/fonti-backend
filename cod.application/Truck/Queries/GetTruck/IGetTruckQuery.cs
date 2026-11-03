using cod.application.Truck.Common;
using System.Threading.Tasks;

namespace cod.application.Truck.Queries.GetTruck
{
    public interface IGetTruckQuery
    {
        Task<TruckModel> Execute(int id);
    }
}
