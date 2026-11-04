using cod.application.PaymentSchedule.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.PaymentSchedule.Commands.AddPaymentScheduleDetails
{
    public interface IAddPaymentScheduleDetailsCommand
    {
        Task<List<int>> Execute(AddPaymentScheduleDetailsModel model);
    }
}
