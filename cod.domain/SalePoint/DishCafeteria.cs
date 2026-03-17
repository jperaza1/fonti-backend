using cod.domain.Common;
using cod.domain.Data;
using cod.domain.Rrhh;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.domain.SalePoint
{
    [Table("DishesCafeteria", Schema = "salepoint")]
    public class DishCafeteria : Entity
    {
        public int DishId { get; set; }
        [ForeignKey("DishId")]
        public virtual Dish Dish { get; set; }
        public int CafeteriaId { get; set; }
        [ForeignKey("CafeteriaId")]
        public virtual Cafeteria Cafeteria { get; set; }
        public SaleTypeDish SaleTypeDish { get; set; }
        public bool NoAddOns { get; set; }
        public int FreeAddOnsQuantity { get; set; }
        public double PriceWithCarnet { get; set; }
        public double PriceWithOutCarnet { get; set; }
    }
}
