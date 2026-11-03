using cod.domain.Common;
using cod.domain.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.domain.Operation
{
    [Table("DispatchControl", Schema = "operation")]
    public class DispatchControl : Entity
    {
        public int RouteId { get; set; }
        [ForeignKey("RouteId")]
        public virtual Route Route { get; set; }
        public int? TruckId { get; set; }
        [ForeignKey("TruckId")]
        public virtual Truck Truck { get; set; }
        public string Vendor { get; set; }
        public DateTime DispatchControlDate { get; set; }
        public DispatchControlStatus Status { get; set; }
        public virtual ICollection<DispatchControlDetail> DispatchControlDetails { get; set; }
    }
}
