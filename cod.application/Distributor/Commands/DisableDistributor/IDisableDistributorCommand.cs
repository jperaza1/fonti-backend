using System.Threading.Tasks;

namespace cod.application.Distributor.Commands.DisableDistributor
{
    public interface IDisableDistributorCommand
    {
        Task Execute(int id);
    }
}
