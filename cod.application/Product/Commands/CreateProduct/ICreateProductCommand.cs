using cod.application.Product.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Product.Commands.CreateProduct
{
    public interface ICreateProductCommand
    {
        Task Execute(ProductModel model);
    }
}
