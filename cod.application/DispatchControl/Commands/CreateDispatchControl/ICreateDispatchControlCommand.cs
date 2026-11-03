using cod.application.DispatchControl.Commom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.DispatchControl.Commands.CreateDispatchControl
{
    public interface ICreateDispatchControlCommand
    {
        Task Execute(DispatchControlModel model);
    }
}
