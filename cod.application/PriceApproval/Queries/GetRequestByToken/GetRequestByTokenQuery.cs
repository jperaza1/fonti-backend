using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.PriceApproval.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.PriceApproval.Queries.GetRequestByToken
{
    public class GetRequestByTokenQuery : BaseQuery, IGetRequestByTokenQuery
    {
        public GetRequestByTokenQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<PriceApprovalModel> Execute(string token)
        {
            var request = await _repository.Query<domain.Operation.PriceApproval>()
                .Where(item => item.Token == token)
                .Select(item => new PriceApprovalModel
                {
                    Id = item.Id,
                    PurchaseOrderId = item.PurchaseOrderId,
                    Token = item.Token,
                    Reference = item.PurchaseOrder.Reference,
                    Vendor = item.PurchaseOrder.Vendor.Name,
                    Status = item.Status,
                    Notes = item.Notes,
                    RejectedUser = item.RejectedUser,
                    RejectedAt = item.RejectedAt,
                    ApprovedUser = item.ApprovedUser,
                    ApprovedAt = item.ApprovedAt,
                    Details = item.PriceApprovalDetails.Select(line => new PriceApprovalDetailModel
                    {
                        Product = line.ProductVendor.Product.Name,
                        OldCost = line.OldCost,
                        NewCost = line.NewCost
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (request == null)
                throw new ApplicationException("Solicitud no existe", "PRECIOS");

            request.StatusName = request.Status.GetEnumDescription();
            return request;
        }
    }
}
