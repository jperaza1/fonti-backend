using cod.application.Product.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Product.Commands.UpdateProduct
{
    public interface IUpdateProductCommand
    {
        Task Execute(ProductModel model, int id);
    }
}
