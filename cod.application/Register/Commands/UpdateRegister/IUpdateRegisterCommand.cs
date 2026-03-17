using cod.application.PurchaseOrder.Common;
using cod.application.Register.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Register.Commands.UpdateRegister
{
    public interface IUpdateRegisterCommand
    {
        Task Execute(RegisterModel model, int id);
    }
}
