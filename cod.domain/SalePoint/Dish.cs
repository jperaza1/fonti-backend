using cod.domain.Common;
using cod.domain.Data;
using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.domain.SalePoint
{
    [Table("Dishes", Schema = "salepoint")]
    public class Dish : Entity
    {
        public string Name { get; set; }
        public DishType DishType { get; set; }
    }
}
