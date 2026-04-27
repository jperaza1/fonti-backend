using cod.application.Customer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Customer.Queries.GetCustomersByCafeteria
{
    public interface IGetCustomersByCafeteriaQuery
    {
        Task<List<CustomerModel>> Execute(int cafeteriaId);
    }
}
