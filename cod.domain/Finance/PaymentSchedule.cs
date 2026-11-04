using cod.domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace cod.domain.Finance
{
    [Table("PaymentSchedule", Schema = "finance")]
    public class PaymentSchedule : Entity
    {
        public DateTime Date { get; set; }
        public string Descripcion { get; set; }
        public virtual ICollection<PaymentScheduleDetail> Details { get; set; }
    }
}
