using cod.application.Customer.Common;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Register.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Customer.Queries.GetCustomers
{
    public class GetCustomersQuery : BaseQuery, IGetCustomersQuery
    {
        public GetCustomersQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<CustomerModel>> Execute()
        {
            return await _repository.Query<domain.Data.Customer>().Select(f => new CustomerModel
            {
                Id = f.Id,
                Name = f.Name,
                Rtn = f.Rtn,
                IsActive = f.IsActive,
            }).ToListAsync();
        }
    }
}
