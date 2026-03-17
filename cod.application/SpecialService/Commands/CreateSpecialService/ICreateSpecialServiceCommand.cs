using cod.application.PurchaseOrder.Common;
using cod.application.SpecialService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.SpecialService.Commands.CreateSpecialService
{
    public interface ICreateSpecialServiceCommand
    {
        Task Execute(SpecialServiceModel model);
    }
}
