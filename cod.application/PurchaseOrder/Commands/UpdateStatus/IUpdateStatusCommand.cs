using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.PurchaseOrder.Commands.UpdateStatus
{
    public interface IUpdateStatusCommand
    {
        Task Execute(int id);
    }
}
