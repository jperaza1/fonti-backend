using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Product.Commands.DisableProduct
{
    public interface IDisableProductCommand
    {
        Task Execute(int id);
    }
}
