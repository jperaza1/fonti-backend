using cod.domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.domain.Data
{
    [Table("Cafeterias", Schema = "data")]
    public class Cafeteria : Entity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int PurchaseOrderCorelative { get; set; }
        public int SpecialServicesCorelative { get; set; }
        public int OfficeRequisitionCorelative { get; set; }
        public int InventoryCorelative { get; set; }
        public virtual ICollection<Register> Registers { get; set; }
        public virtual ICollection<CustomerCafeteria> CustomerCafeterias { get; set; }
    }
}
