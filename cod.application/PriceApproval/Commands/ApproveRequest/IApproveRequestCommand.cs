using cod.application.PriceApproval.Common;
using System.Threading.Tasks;

namespace cod.application.PriceApproval.Commands.ApproveRequest
{
    public interface IApproveRequestCommand
    {
        Task Execute(ApproveRequestModel model);
    }
}
