using System.Threading.Tasks;

namespace cod.application.Truck.Commands.DisableTruck
{
    public interface IDisableTruckCommand
    {
        Task Execute(int id);
    }
}
