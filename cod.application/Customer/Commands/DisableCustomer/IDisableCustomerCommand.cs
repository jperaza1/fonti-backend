using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Customer.Commands.DisableCustomer
{
    public interface IDisableCustomerCommand
    {
        Task Execute(int id);
    }
}
