using cod.application.Truck.Common;
using System.Threading.Tasks;

namespace cod.application.Truck.Commands.UpdateTruck
{
    public interface IUpdateTruckCommand
    {
        Task Execute(TruckModel model, int id);
    }
}
