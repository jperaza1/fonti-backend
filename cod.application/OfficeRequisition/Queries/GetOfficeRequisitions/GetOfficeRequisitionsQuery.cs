using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.OfficeRequisition.Common;
using cod.application.SpecialService.Common;
using cod.application.SpecialService.Queries.GetSpecialServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.OfficeRequisition.Queries.GetOfficeRequisitions
{
    public class GetOfficeRequisitionsQuery : BaseQuery, IGetOfficeRequisitionsQuery
    {
        public GetOfficeRequisitionsQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<OfficeRequisitionModel>> Execute(FilterOfficeRequisition filterModel)
        {
            Expression<Func<domain.Operation.OfficeRequisition, bool>> GetSpecialServiceByCafeteriaFilter = GetSpecialServiceByCafeteria(filterModel);
            Expression<Func<domain.Operation.OfficeRequisition, bool>> GetSpecialServiceByDateFilter = GetSpecialServiceByDate(filterModel);
            Expression<Func<domain.Operation.OfficeRequisition, bool>> GetSpecialServiceByStatusFilter = GetSpecialServiceByStatus(filterModel);

            var baseQuery = _repository.Query<domain.Operation.OfficeRequisition>()
            .Where(GetSpecialServiceByDateFilter)
            .Where(GetSpecialServiceByCafeteriaFilter)
            .Where(GetSpecialServiceByStatusFilter)
            .Select(f => new
            {
                f.Id,
                f.CafeteriaId,
                Cafeteria = f.Cafeteria.Name,
                f.Date,
                f.Reference,
                f.Status,
                f.CreatorUser,
                f.CreationTime,
                f.ApprovedUser,
                f.ApprovedAt,
            });

            return await baseQuery.Select(purchaseOrder => new OfficeRequisitionModel
            {
                Id = purchaseOrder.Id,
                CafeteriaId = purchaseOrder.CafeteriaId,
                Cafeteria = purchaseOrder.Cafeteria,
                Date = purchaseOrder.Date,
                Reference = purchaseOrder.Reference,
                StatusName = purchaseOrder.Status.GetEnumDescription(),
                Status = purchaseOrder.Status,
                CreatorUser = purchaseOrder.CreatorUser,
                ApprovedAt = purchaseOrder.ApprovedAt,
                CreationTime = purchaseOrder.CreationTime,
                ApprovedUser = purchaseOrder.ApprovedUser,
            }).ToListAsync();
        }

        public static Expression<Func<domain.Operation.OfficeRequisition, bool>> GetSpecialServiceByCafeteria(FilterOfficeRequisition filterModel)
        {
            if (filterModel.CafeteriaId.HasValue && filterModel.CafeteriaId > 0)
                return detail => detail.CafeteriaId == filterModel.CafeteriaId;
            return detail => true;
        }

        public static Expression<Func<domain.Operation.OfficeRequisition, bool>> GetSpecialServiceByDate(FilterOfficeRequisition filterModel)
        {
            if (filterModel.StartDate.HasValue && filterModel.EndDate.HasValue)
                return invoice => invoice.Date.Date >= filterModel.StartDate.Value.ToUniversalTime().Date && invoice.Date.Date <= filterModel.EndDate.Value.ToUniversalTime().Date;
            return invoice => true;
        }

        public static Expression<Func<domain.Operation.OfficeRequisition, bool>> GetSpecialServiceByStatus(FilterOfficeRequisition filterModel)
        {
            if (filterModel.Status.IsValid())
                return invoice => invoice.Status == filterModel.Status;
            return invoice => true;
        }
    }
}
