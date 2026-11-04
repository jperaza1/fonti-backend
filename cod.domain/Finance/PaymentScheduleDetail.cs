using cod.domain.Common;
using System.ComponentModel.DataAnnotations.Schema;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.domain.Finance
{
    [Table("PaymentScheduleDetails", Schema = "finance")]
    public class PaymentScheduleDetail : Entity
    {
        public int PaymentScheduleId { get; set; }
        [ForeignKey("PaymentScheduleId")]
        public virtual PaymentSchedule PaymentSchedule { get; set; }
        public string Vendor { get; set; }
        public string Descripcion { get; set; }
        public double AccumulatedAmount { get; set; }
        public double ProjectedAmount { get; set; }
        public double RealAmount { get; set; }
        public PaymentScheduleStatus Status { get; set; }
    }
}
