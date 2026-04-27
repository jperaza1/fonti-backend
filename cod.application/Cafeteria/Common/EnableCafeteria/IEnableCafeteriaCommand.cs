using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Cafeteria.Common.EnableCafeteria
{
    public interface IEnableCafeteriaCommand
    {
        Task Execute(int id);
    }
}
