using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Register.Commands.EnableRegister
{
    public interface IEnableRegisterCommand
    {
        Task Execute(int id);
    }
}
