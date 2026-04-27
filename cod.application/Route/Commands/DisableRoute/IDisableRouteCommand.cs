using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Route.Commands.DisableRoute
{
    public interface IDisableRouteCommand
    {
        Task Execute(int id);
    }
}
