using cod.application.Customer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Customer.Commands.UpdateCustomer
{
    public interface IUpdateCustomerCommand
    {
        Task Execute(CustomerModel model, int id);
    }
}
