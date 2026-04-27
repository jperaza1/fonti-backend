using cod.application.Dish.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Dish.Commands.AddDishCafeteria
{
    public interface IAddDishCafeteriaCommand
    {
        Task Execute(DishCafeteriaModel model);
    }
}
