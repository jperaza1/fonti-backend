using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.Dish.Common
{
    public class DishCafeteriaModel
    {
        public int Id { get; set; }
        public int DishId { get; set; }
        public string DishName { get; set; }
        public int CafeteriaId { get; set; }
        public string Cafeteria { get; set; }
        public SaleTypeDish SaleTypeDish { get; set; }
        public string SaleTypeDishName { get; set; }
        public bool NoAddOns { get; set; }
        public int FreeAddOnsQuantity { get; set; }
        public double PriceWithCarnet { get; set; }
        public double PriceWithOutCarnet { get; set; }
        public bool IsActive { get; set; }
    }
}
