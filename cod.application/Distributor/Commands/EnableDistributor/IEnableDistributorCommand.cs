using System.Threading.Tasks;

namespace cod.application.Distributor.Commands.EnableDistributor
{
    public interface IEnableDistributorCommand
    {
        Task Execute(int id);
    }
}
