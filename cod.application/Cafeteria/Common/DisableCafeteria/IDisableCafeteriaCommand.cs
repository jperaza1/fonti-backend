using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Cafeteria.Common.DisableCafeteria
{
    public interface IDisableCafeteriaCommand
    {
        Task Execute(int id);
    }
}
