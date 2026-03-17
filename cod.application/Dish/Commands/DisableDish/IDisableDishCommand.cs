using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Dish.Commands.DisableDish
{
    public interface IDisableDishCommand
    {
        Task Execute(int id);
    }
}
