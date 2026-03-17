using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Sale.Commands.DisableSale
{
    public interface IDisableSaleCommand
    {
        Task Execute(int id);
    }
}
