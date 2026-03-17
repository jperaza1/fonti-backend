using cod.application.Customer.Common;
using cod.application.Features.Base;
using cod.application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Customer.Queries.GetActiveCustomers
{
    public class GetActiveCustomersQuery : BaseQuery, IGetActiveCustomersQuery
    {
        public GetActiveCustomersQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<CustomerModel>> Execute()
        {
            return await _repository.QueryActive<domain.Data.Customer>().Select(f => new CustomerModel
            {
                Id = f.Id,
                Name = f.Name,
                Rtn = f.Rtn,
                IsActive = f.IsActive,
            }).ToListAsync();
        }
    }
}
