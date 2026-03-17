using cod.application.Customer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Customer.Queries.GetActiveCustomers
{
    public interface IGetActiveCustomersQuery
    {
        Task<List<CustomerModel>> Execute();
    }
}
