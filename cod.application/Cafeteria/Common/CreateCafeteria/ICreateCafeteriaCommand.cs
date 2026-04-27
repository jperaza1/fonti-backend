using cod.application.Cafeteria.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Cafeteria.Common.CreateCafeteria
{
    public interface ICreateCafeteriaCommand
    {
        Task Execute(CafeteriaModel model);
    }
}
