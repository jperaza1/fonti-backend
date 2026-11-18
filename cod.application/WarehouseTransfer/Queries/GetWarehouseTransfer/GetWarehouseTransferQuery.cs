using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.WarehouseTransfer.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.WarehouseTransfer.Queries.GetWarehouseTransfer
{
    public class GetWarehouseTransferQuery : BaseQuery, IGetWarehouseTransferQuery
    {
        public GetWarehouseTransferQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<WarehouseTransferModel> Execute(int id)
        {
            var transfer = await _repository.QueryActive<domain.Operation.WarehouseTransfer>()
                .Include(item => item.FromWarehouse)
                .Include(item => item.ToWarehouse)
                .Include(item => item.Details)
                .ThenInclude(detail => detail.Product)
                .FirstOrDefaultAsync(item => item.Id == id);

            if (transfer == null)
                return null;

            return new WarehouseTransferModel
            {
                Id = transfer.Id,
                Date = transfer.Date,
                FromWarehouseId = transfer.FromWarehouseId,
                FromWarehouse = transfer.FromWarehouse == null ? null : transfer.FromWarehouse.Name,
                ToWarehouseId = transfer.ToWarehouseId,
                ToWarehouse = transfer.ToWarehouse == null ? null : transfer.ToWarehouse.Name,
                Note = transfer.Note,
                Status = transfer.Status,
                StatusName = transfer.Status.GetEnumDescription(),
                SentUser = transfer.SentUser,
                Reason = transfer.Reason,
                Details = transfer.Details
                    .Where(detail => detail.IsActive)
                    .Select(detail => new WarehouseTransferDetailModel
                    {
                        Id = detail.Id,
                        ProductId = detail.ProductId,
                        Product = detail.Product == null ? null : detail.Product.Name,
                        Quantity = detail.Quantity
                    })
                    .ToList()
            };
        }
    }
}
