using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.SpecialService.Commands.UpdateStatus
{
    public interface IUpdateStatusSpecialServiceCommand
    {
        Task Execute(int id);
    }
}
