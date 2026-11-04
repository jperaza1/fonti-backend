using cod.application.PaymentSchedule.Common;
using System.Threading.Tasks;

namespace cod.application.PaymentSchedule.Commands.CreatePaymentSchedule
{
    public interface ICreatePaymentScheduleCommand
    {
        Task<int> Execute(PaymentScheduleModel model);
    }
}
