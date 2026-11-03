using cod.application.PriceApproval.Common;
using System.Threading.Tasks;

namespace cod.application.PriceApproval.Queries.GetRequestByToken
{
    public interface IGetRequestByTokenQuery
    {
        Task<PriceApprovalModel> Execute(string token);
    }
}
