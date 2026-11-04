using System;
using System.Collections.Generic;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.PaymentSchedule.Common
{
    public class PaymentScheduleModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Descripcion { get; set; }
        public int DetailCount { get; set; }
        public double AccumulatedAmount { get; set; }
        public double ProjectedAmount { get; set; }
        public double RealAmount { get; set; }
        public double PendingAmount { get; set; }
        public bool CanAddDetails { get; set; }
        public List<PaymentScheduleDetailModel> Details { get; set; }
    }

    public class PaymentScheduleDetailModel
    {
        public int Id { get; set; }
        public int PaymentScheduleId { get; set; }
        public string Vendor { get; set; }
        public string Descripcion { get; set; }
        public double AccumulatedAmount { get; set; }
        public double ProjectedAmount { get; set; }
        public double RealAmount { get; set; }
        public double PendingAmount { get; set; }
        public PaymentScheduleStatus Status { get; set; }
        public string StatusName { get; set; }
        public bool FromPreviousSchedule { get; set; }
    }

    public class PaymentScheduleCarryForwardModel
    {
        public int? PreviousScheduleId { get; set; }
        public DateTime? PreviousDate { get; set; }
        public string PreviousDescripcion { get; set; }
        public List<PaymentScheduleDetailModel> Details { get; set; }
    }

    public class RegisterPaymentScheduleDetailModel
    {
        public int DetailId { get; set; }
        public double RealAmount { get; set; }
    }

    public class AddPaymentScheduleDetailsModel
    {
        public int PaymentScheduleId { get; set; }
        public List<PaymentScheduleDetailModel> Details { get; set; }
    }
}
