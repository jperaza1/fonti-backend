using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Dish.Commands.RemoveDishCafeteria
{
    public interface IRemoveDishCafeteriaCommand
    {
        Task Execute(int id);
    }
}
