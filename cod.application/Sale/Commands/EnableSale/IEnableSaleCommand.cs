using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Sale.Commands.EnableSale
{
    public interface IEnableSaleCommand
    {
        Task Execute(int id);
    }
}
