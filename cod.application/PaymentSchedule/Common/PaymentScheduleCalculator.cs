using System;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.PaymentSchedule.Common
{
    public static class PaymentScheduleCalculator
    {
        public static double Due(double accumulatedAmount, double projectedAmount)
        {
            return Math.Round(Math.Max(accumulatedAmount, projectedAmount), 2);
        }

        public static double Pending(double accumulatedAmount, double projectedAmount, double realAmount)
        {
            return Math.Round(Math.Max(0, Due(accumulatedAmount, projectedAmount) - realAmount), 2);
        }

        public static PaymentScheduleStatus Status(double accumulatedAmount, double projectedAmount, double realAmount)
        {
            var due = Due(accumulatedAmount, projectedAmount);
            var paid = Math.Round(realAmount, 2);

            if (due <= 0 || paid >= due)
                return PaymentScheduleStatus.Paid;

            if (paid <= 0)
                return PaymentScheduleStatus.Pending;

            return PaymentScheduleStatus.Partial;
        }
    }
}
