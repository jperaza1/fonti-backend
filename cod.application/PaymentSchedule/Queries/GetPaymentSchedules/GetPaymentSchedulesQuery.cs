using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.PaymentSchedule.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.PaymentSchedule.Queries.GetPaymentSchedules
{
    public class GetPaymentSchedulesQuery : BaseQuery, IGetPaymentSchedulesQuery
    {
        public GetPaymentSchedulesQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<PaymentScheduleModel>> Execute()
        {
            var schedules = await _repository.QueryActive<domain.Finance.PaymentSchedule>()
                .OrderByDescending(f => f.Date)
                .ThenByDescending(f => f.Id)
                .Select(f => new
                {
                    f.Id,
                    f.Date,
                    f.Descripcion,
                    Details = f.Details
                        .Where(d => d.IsActive)
                        .Select(d => new
                        {
                            d.AccumulatedAmount,
                            d.ProjectedAmount,
                            d.RealAmount
                        })
                        .ToList()
                })
                .ToListAsync();

            return schedules.Select((f, index) =>
            {
                var pending = f.Details.Sum(d => PaymentScheduleCalculator.Pending(d.AccumulatedAmount, d.ProjectedAmount, d.RealAmount));
                return new PaymentScheduleModel
                {
                    Id = f.Id,
                    Date = f.Date,
                    Descripcion = f.Descripcion,
                    DetailCount = f.Details.Count,
                    AccumulatedAmount = f.Details.Sum(d => d.AccumulatedAmount),
                    ProjectedAmount = f.Details.Sum(d => d.ProjectedAmount),
                    RealAmount = f.Details.Sum(d => d.RealAmount),
                    PendingAmount = pending,
                    CanAddDetails = index == 0,
                    Details = new List<PaymentScheduleDetailModel>()
                };
            }).ToList();
        }
    }
}
