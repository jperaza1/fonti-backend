using cod.application.Common.Clases;
using cod.application.Common.Extensions;
using cod.application.DispatchControl.Commom;
using cod.application.Expense.Common;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.DispatchControl.Queries.GetAllDispatchControl
{
    public class GetAllDispatchControlQuery : BaseQuery, IGetAllDispatchControlQuery
    {
        private readonly AuthService _authService;
        public GetAllDispatchControlQuery(
            IRepository repository, AuthService authService) : base(repository)
        {
            _authService = authService;
        }

        public async Task<List<DispatchControlModel>> Execute(DispatchControlFilterModel filterModel)
        {
            var timezone = _authService.ClientTimezone;

            Expression<Func<domain.Operation.DispatchControl, bool>> byDate = GetDispatchControlByDate(filterModel, timezone);

            var baseQuery = _repository.Query<domain.Operation.DispatchControl>()
                .Where(byDate)
                .Select(f => new
                {
                    f.Id,
                    f.RouteId,
                    RouteName = f.Route.Name,
                    f.Vendor,
                    f.TruckId,
                    Truck = f.TruckId.HasValue ? f.Truck.Plate : string.Empty,
                    f.DispatchControlDate,
                    f.CreationTime,
                    f.CreatorUser,
                    f.Status,
                });

            return await baseQuery.Select(f => new DispatchControlModel
            {
                Id = f.Id,
                DispatchControlDate = f.DispatchControlDate,
                Route = f.RouteName,
                RouteId = f.RouteId,
                Vendor = f.Vendor,
                TruckId = f.TruckId,
                Truck = f.Truck,
                CreationTime = f.CreationTime,
                CreatorUser = f.CreatorUser,
                Status = f.Status,
                StatusName = f.Status.GetEnumDescription()
            }).ToListAsync();
        }

        public static Expression<Func<domain.Operation.DispatchControl, bool>> GetDispatchControlByDate(DispatchControlFilterModel filterModel, string timeZone)
        {
            if (filterModel.DateFrom.HasValue && filterModel.DateTo.HasValue)
            {
                filterModel.DateFrom = DateService.GetCLientLocalTime(filterModel.DateFrom.Value, timeZone);
                filterModel.DateTo = DateService.GetCLientLocalTime(filterModel.DateTo.Value, timeZone);
                return e => e.DispatchControlDate.Date >= filterModel.DateFrom.Value.Date && e.DispatchControlDate.Date <= filterModel.DateTo.Value.Date;
            }

            return e => true;
        }
    }
}
