using cod.application.Dish.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Dish.Commands.CreateDish
{
    public interface ICreateDishCommand
    {
        Task Execute(DishModel model);
    }
}
