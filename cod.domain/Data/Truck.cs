using cod.domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.domain.Data
{
    [Table("Trucks", Schema = "data")]
    public class Truck : Entity
    {
        public string Name { get; set; }
        public string Plate { get; set; }
        public TruckStatus TruckStatus { get; set; }
        public int? RouteId { get; set; }
        [ForeignKey("RouteId")]
        public virtual Route Route { get; set; }
    }
}
