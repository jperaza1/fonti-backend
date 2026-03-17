using cod.application.Customer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Customer.Commands.CreateCustomer
{
    public interface ICreateCustomerCommand
    {
        Task Execute(CustomerModel model);
    }
}
