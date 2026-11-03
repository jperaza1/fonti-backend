using System.Threading.Tasks;

namespace cod.application.Truck.Commands.EnableTruck
{
    public interface IEnableTruckCommand
    {
        Task Execute(int id);
    }
}
