using cod.application.Dish.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Dish.Commands.UpdateDish
{
    public interface IUpdateDishCommand
    {
        Task Execute(DishModel model, int id);
    }
}
