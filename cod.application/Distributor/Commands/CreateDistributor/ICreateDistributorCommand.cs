using cod.application.Distributor.Common;
using System.Threading.Tasks;

namespace cod.application.Distributor.Commands.CreateDistributor
{
    public interface ICreateDistributorCommand
    {
        Task Execute(DistributorModel model);
    }
}
