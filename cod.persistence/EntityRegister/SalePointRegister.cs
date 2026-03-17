using cod.domain.SalePoint;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.persistence.Implementations
{
    public partial class Repository
    {
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<SideItem> SideItems { get; set; }
        public DbSet<DishCafeteria> DishesCafeteria { get; set; }
    }
}
