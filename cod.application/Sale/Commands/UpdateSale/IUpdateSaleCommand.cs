using cod.application.Sale.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Sale.Commands.UpdateSale
{
    public interface IUpdateSaleCommand
    {
        Task Execute(SaleModel model, int id);
    }
}
