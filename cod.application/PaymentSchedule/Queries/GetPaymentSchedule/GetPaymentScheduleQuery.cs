using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.PaymentSchedule.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.PaymentSchedule.Queries.GetPaymentSchedule
{
    public class GetPaymentScheduleQuery : BaseQuery, IGetPaymentScheduleQuery
    {
        public GetPaymentScheduleQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<PaymentScheduleModel> Execute(int id)
        {
            var schedule = await _repository.QueryActive<domain.Finance.PaymentSchedule>()
                .Where(f => f.Id == id)
                .Select(f => new
                {
                    f.Id,
                    f.Date,
                    f.Descripcion,
                    Details = f.Details
                        .Where(d => d.IsActive)
                        .OrderBy(d => d.Vendor)
                        .Select(d => new
                        {
                            d.Id,
                            d.PaymentScheduleId,
                            d.Vendor,
                            d.Descripcion,
                            d.AccumulatedAmount,
                            d.ProjectedAmount,
                            d.RealAmount,
                            d.Status
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (schedule == null)
                throw new ApplicationException("Programacion no encontrada");

            var canAddDetails = !await _repository.QueryActive<domain.Finance.PaymentSchedule>()
                .AnyAsync(f => f.Date > schedule.Date || (f.Date == schedule.Date && f.Id > schedule.Id));

            var details = schedule.Details.Select(d => new PaymentScheduleDetailModel
            {
                Id = d.Id,
                PaymentScheduleId = d.PaymentScheduleId,
                Vendor = d.Vendor,
                Descripcion = d.Descripcion,
                AccumulatedAmount = d.AccumulatedAmount,
                ProjectedAmount = d.ProjectedAmount,
                RealAmount = d.RealAmount,
                PendingAmount = PaymentScheduleCalculator.Pending(d.AccumulatedAmount, d.ProjectedAmount, d.RealAmount),
                Status = d.Status,
                StatusName = d.Status.GetEnumDescription()
            }).ToList();

            return new PaymentScheduleModel
            {
                Id = schedule.Id,
                Date = schedule.Date,
                Descripcion = schedule.Descripcion,
                DetailCount = details.Count,
                AccumulatedAmount = details.Sum(d => d.AccumulatedAmount),
                ProjectedAmount = details.Sum(d => d.ProjectedAmount),
                RealAmount = details.Sum(d => d.RealAmount),
                PendingAmount = details.Sum(d => d.PendingAmount),
                CanAddDetails = canAddDetails,
                Details = details
            };
        }
    }
}
