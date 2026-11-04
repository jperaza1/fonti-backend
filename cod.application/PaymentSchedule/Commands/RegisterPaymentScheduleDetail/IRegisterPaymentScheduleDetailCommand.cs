using cod.application.PaymentSchedule.Common;
using System.Threading.Tasks;

namespace cod.application.PaymentSchedule.Commands.RegisterPaymentScheduleDetail
{
    public interface IRegisterPaymentScheduleDetailCommand
    {
        Task Execute(RegisterPaymentScheduleDetailModel model);
    }
}
