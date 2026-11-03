using cod.application.Truck.Common;
using System.Threading.Tasks;

namespace cod.application.Truck.Commands.CreateTruck
{
    public interface ICreateTruckCommand
    {
        Task Execute(TruckModel model);
    }
}
