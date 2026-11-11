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
    [Table("DispatchControlDetail", Schema = "operation")]
    public class DispatchControlDetail : Entity
    {
        public int DispatchControlId { get; set; }
        [ForeignKey("DispatchControlId")]
        public virtual DispatchControl DispatchControl { get; set; }
        public DispatchControlType DispatchControlType { get; set; }
        public int BottleQuantity { get; set; }
        public int CrateQuantity { get; set; }
        public int MissingBottleQuantity { get; set; }
        public int? DispatchTransferId { get; set; }
        [ForeignKey("DispatchTransferId")]
        public virtual DispatchTransfer DispatchTransfer { get; set; }
        public int? CounterpartRouteId { get; set; }
        [ForeignKey("CounterpartRouteId")]
        public virtual Route CounterpartRoute { get; set; }
        public virtual ICollection<ProductMovement> ProductMovements { get; set; }
    }


}
