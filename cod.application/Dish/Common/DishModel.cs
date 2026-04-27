using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.Dish.Common
{
    public class DishModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DishType DishType { get; set; }
        public string DishTypeName { get; set; }
        public bool IsActive { get; set; }
    }
}
