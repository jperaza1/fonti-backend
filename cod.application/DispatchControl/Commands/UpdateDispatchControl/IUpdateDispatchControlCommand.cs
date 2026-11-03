using cod.application.DispatchControl.Commom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.DispatchControl.Commands.UpdateDispatchControl
{
    public interface IUpdateDispatchControlCommand
    {
        Task Execute(DispatchControlModel model, int id);
    }
}
