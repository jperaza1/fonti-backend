using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Product.Commands.EnableProduct
{
    public interface IEnableProductCommand
    {
        Task Execute(int id);
    }
}
