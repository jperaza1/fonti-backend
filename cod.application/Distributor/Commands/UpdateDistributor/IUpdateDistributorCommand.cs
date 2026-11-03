using cod.application.Distributor.Common;
using System.Threading.Tasks;

namespace cod.application.Distributor.Commands.UpdateDistributor
{
    public interface IUpdateDistributorCommand
    {
        Task Execute(DistributorModel model, int id);
    }
}
