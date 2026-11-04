using cod.application.PaymentSchedule.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.PaymentSchedule.Queries.GetPaymentSchedules
{
    public interface IGetPaymentSchedulesQuery
    {
        Task<List<PaymentScheduleModel>> Execute();
    }
}
