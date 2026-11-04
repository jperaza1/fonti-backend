using cod.application.PaymentSchedule.Common;
using System.Threading.Tasks;

namespace cod.application.PaymentSchedule.Queries.GetPaymentSchedule
{
    public interface IGetPaymentScheduleQuery
    {
        Task<PaymentScheduleModel> Execute(int id);
    }
}
