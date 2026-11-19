using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.WarehouseTransfer.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.WarehouseTransfer.Queries.GetWarehouseTransfers
{
    public class GetWarehouseTransfersQuery : BaseQuery, IGetWarehouseTransfersQuery
    {
        public GetWarehouseTransfersQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<WarehouseTransferModel>> Execute(DateTime? dateFrom, DateTime? dateTo)
        {
            var query = _repository.QueryActive<domain.Operation.WarehouseTransfer>();

            if (dateFrom.HasValue)
            {
                var start = dateFrom.Value.Date;
                query = query.Where(item => item.Date >= start);
            }

            if (dateTo.HasValue)
            {
                var end = dateTo.Value.Date.AddDays(1);
                query = query.Where(item => item.Date < end);
            }

            var rows = await query
                .OrderByDescending(item => item.Date)
                .ThenByDescending(item => item.Id)
                .Select(item => new
                {
                    item.Id,
                    item.Date,
                    item.FromWarehouseId,
                    FromWarehouse = item.FromWarehouse.Name,
                    item.ToWarehouseId,
                    ToWarehouse = item.ToWarehouse.Name,
                    item.Note,
                    item.Status,
                    item.SentUser,
                    item.Reason
                })
                .ToListAsync();

            return rows.Select(item => new WarehouseTransferModel
            {
                Id = item.Id,
                Date = item.Date,
                FromWarehouseId = item.FromWarehouseId,
                FromWarehouse = item.FromWarehouse,
                ToWarehouseId = item.ToWarehouseId,
                ToWarehouse = item.ToWarehouse,
                Note = item.Note,
                Status = item.Status,
                StatusName = item.Status.GetEnumDescription(),
                SentUser = item.SentUser,
                Reason = item.Reason
            }).ToList();
        }
    }
}
