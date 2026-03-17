using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Register.Commands.DisableRegister
{
    public interface IDisableRegisterCommand
    {
        Task Execute(int id);
    }
}
