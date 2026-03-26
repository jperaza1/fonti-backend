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
    [Table("Sales", Schema = "operation")]
    public class Sale : Entity
    {
        public int RouteId { get; set; }
        [ForeignKey("RouteId")]
        public virtual Route Route { get; set; }
        public SaleType SaleType { get; set; }
        public DateTime SaleDate { get; set; }
        public double Amount { get; set; }

        public virtual ICollection<SaleDetail> SaleDetails { get; set; }
    }
}
