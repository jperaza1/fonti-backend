using cod.application.PaymentSchedule.Common;
using System.Threading.Tasks;

namespace cod.application.PaymentSchedule.Queries.GetPaymentScheduleCarryForward
{
    public interface IGetPaymentScheduleCarryForwardQuery
    {
        Task<PaymentScheduleCarryForwardModel> Execute();
    }
}
