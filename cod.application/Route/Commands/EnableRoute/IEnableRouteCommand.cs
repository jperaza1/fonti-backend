using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Route.Commands.EnableRoute
{
    public interface IEnableRouteCommand
    {
        Task Execute(int id);
    }
}
