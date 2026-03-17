using cod.application.PurchaseOrder.Common;
using cod.application.SpecialService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.SpecialService.Commands.UpdateSpecialService
{
    public interface IUpdateSpecialServiceCommand
    {
        Task Execute(SpecialServiceModel model, int id);
    }
}
