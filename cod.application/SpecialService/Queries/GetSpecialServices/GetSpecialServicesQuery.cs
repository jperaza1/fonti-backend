using cod.application.Common.Clases;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.PurchaseOrder.Common;
using cod.application.PurchaseOrder.Queries.GetPurchasesOrder;
using cod.application.SpecialService.Common;
using cod.domain.Features;
using DocuSign.eSign.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.SpecialService.Queries.GetSpecialServices
{
    public class GetSpecialServicesQuery : BaseQuery, IGetSpecialServicesQuery
    {
        private readonly AuthService _authService;
        public GetSpecialServicesQuery(IRepository repository, AuthService authService) : base(repository)
        {
            _authService = authService;
        }

        public async Task<List<SpecialServiceModel>> Execute(FilterSpecialService filterModel)
        {
            var timezone = _authService.ClientTimezone;

            Expression<Func<domain.Operation.SpecialService, bool>> GetSpecialServiceByCafeteriaFilter = GetSpecialServiceByCafeteria(filterModel);
            Expression<Func<domain.Operation.SpecialService, bool>> GetSpecialServiceByDateFilter = GetSpecialServiceByDate(filterModel, timezone);
            Expression<Func<domain.Operation.SpecialService, bool>> GetSpecialServiceByStatusFilter = GetSpecialServiceByStatus(filterModel);

            var baseQuery = _repository.Query<domain.Operation.SpecialService>()
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

            return await baseQuery.Select(purchaseOrder => new SpecialServiceModel
            {
                Id = purchaseOrder.Id,
                CafeteriaId = purchaseOrder.CafeteriaId,
                Cafeteria = purchaseOrder.Cafeteria,
                Date = purchaseOrder.Date.Date,
                Reference = purchaseOrder.Reference,
                StatusName = purchaseOrder.Status.GetEnumDescription(),
                Status = purchaseOrder.Status,
                CreatorUser = purchaseOrder.CreatorUser,
                ApprovedAt = purchaseOrder.ApprovedAt,
                CreationTime = purchaseOrder.CreationTime,
                ApprovedUser = purchaseOrder.ApprovedUser,
            }).ToListAsync();
        }

        public static Expression<Func<domain.Operation.SpecialService, bool>> GetSpecialServiceByCafeteria(FilterSpecialService filterModel)
        {
            if (filterModel.CafeteriaId.HasValue && filterModel.CafeteriaId > 0)
                return detail => detail.CafeteriaId == filterModel.CafeteriaId;
            return detail => true;
        }

        public static Expression<Func<domain.Operation.SpecialService, bool>> GetSpecialServiceByDate(FilterSpecialService filterModel, string timeZone)
        {
            filterModel.StartDate = DateService.GetCLientLocalTime(filterModel.StartDate.Value, timeZone);
            filterModel.EndDate = DateService.GetCLientLocalTime(filterModel.EndDate.Value, timeZone);
            if (filterModel.StartDate.HasValue && filterModel.EndDate.HasValue)
                return invoice => invoice.Date.Date >= filterModel.StartDate.Value.Date && invoice.Date.Date <= filterModel.EndDate.Value.Date;
            return invoice => true;
        }

        public static Expression<Func<domain.Operation.SpecialService, bool>> GetSpecialServiceByStatus(FilterSpecialService filterModel)
        {
            if (filterModel.Status.IsValid())
                return invoice => invoice.Status == filterModel.Status;
            return invoice => true;
        }
    }
}
