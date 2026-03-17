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

namespace cod.application.Customer.Queries.GetCustomersByCafeteria
{
    public class GetCustomersByCafeteriaQuery : BaseQuery, IGetCustomersByCafeteriaQuery
    {
        public GetCustomersByCafeteriaQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<CustomerModel>> Execute(int cafeteriaId)
        {
            var baseQuery = _repository.QueryActive<domain.Data.CustomerCafeteria>()
                .Where(f => f.CafeteriaId == cafeteriaId);

            var result = await baseQuery.Select(f => new CustomerModel
            {
                Id = f.CustomerId,
                Name = f.Customer.Name,
                Rtn = f.Customer.Rtn,
            }).ToListAsync();

            return result;
        }
    }
}
