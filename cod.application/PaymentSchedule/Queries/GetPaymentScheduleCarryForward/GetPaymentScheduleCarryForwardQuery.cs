using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.PaymentSchedule.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.PaymentSchedule.Queries.GetPaymentScheduleCarryForward
{
    public class GetPaymentScheduleCarryForwardQuery : BaseQuery, IGetPaymentScheduleCarryForwardQuery
    {
        public GetPaymentScheduleCarryForwardQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<PaymentScheduleCarryForwardModel> Execute()
        {
            var last = await _repository.QueryActive<domain.Finance.PaymentSchedule>()
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
                            d.Vendor,
                            d.Descripcion,
                            d.AccumulatedAmount,
                            d.ProjectedAmount,
                            d.RealAmount
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            var result = new PaymentScheduleCarryForwardModel
            {
                Details = new System.Collections.Generic.List<PaymentScheduleDetailModel>()
            };

            if (last == null)
                return result;

            result.PreviousScheduleId = last.Id;
            result.PreviousDate = last.Date;
            result.PreviousDescripcion = last.Descripcion;
            result.Details = last.Details
                .Select(d => new
                {
                    Vendor = d.Vendor == null ? string.Empty : d.Vendor.Trim(),
                    Descripcion = string.IsNullOrWhiteSpace(d.Descripcion) ? null : d.Descripcion.Trim(),
                    Pending = PaymentScheduleCalculator.Pending(d.AccumulatedAmount, d.ProjectedAmount, d.RealAmount)
                })
                .Where(d => d.Pending > 0 && d.Vendor != string.Empty)
                .GroupBy(d => d.Vendor.ToUpper())
                .Select(g => new PaymentScheduleDetailModel
                {
                    Vendor = g.First().Vendor,
                    Descripcion = g.Select(x => x.Descripcion).FirstOrDefault(x => !string.IsNullOrWhiteSpace(x)),
                    AccumulatedAmount = g.Sum(x => x.Pending),
                    ProjectedAmount = g.Sum(x => x.Pending),
                    RealAmount = 0,
                    PendingAmount = g.Sum(x => x.Pending),
                    Status = PaymentScheduleStatus.Pending,
                    StatusName = "Pendiente",
                    FromPreviousSchedule = true
                })
                .OrderBy(d => d.Vendor)
                .ToList();

            return result;
        }
    }
}
