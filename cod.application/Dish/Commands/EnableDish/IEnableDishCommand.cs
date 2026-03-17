using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Dish.Commands.EnableDish
{
    public interface IEnableDishCommand
    {
        Task Execute(int id);
    }
}
