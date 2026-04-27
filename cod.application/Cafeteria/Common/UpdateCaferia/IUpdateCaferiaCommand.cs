using cod.application.Cafeteria.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Cafeteria.Common.UpdateCaferia
{
    public interface IUpdateCaferiaCommand
    {
        Task Execute(CafeteriaModel model, int id);
    }
}
