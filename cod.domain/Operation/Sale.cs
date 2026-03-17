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
        public int CafeteriaId { get; set; }
        [ForeignKey("CafeteriaId")]
        public virtual Cafeteria Cafeteria { get; set; }
        public SaleType SaleType { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Data.Type Category { get; set; }
        public int ProductTypeId { get; set; }
        [ForeignKey("ProductTypeId")]
        public int? RegisterId { get; set; }
        [ForeignKey("RegisterId")]
        public virtual Register Register { get; set; }
        public int? ServicePeriodId { get; set; }
        [ForeignKey("ServicePeriodId")]
        public virtual Data.Type ServicePeriod { get; set; }
        public virtual Data.Type ProductType { get; set; }
        public int? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        public DateTime SaleDate { get; set; }
        public double Amount { get; set; }
    }
}
