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
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Customer.Queries.GetCustomer
{
    public class GetCustomerQuery : BaseQuery, IGetCustomerQuery
    {
        public GetCustomerQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<CustomerModel> Execute(int id)
        {
            var register = await _repository.AllAsync<domain.Data.Customer>().Where(f => f.Id == id).FirstOrDefaultAsync();

            if (register == null)
                throw new ApplicationException("Cliente no existe", "CLIENTE");

            return new CustomerModel
            {
                Id = register.Id,
                Name = register.Name,
                Rtn = register.Rtn,
                IsActive = register.IsActive,
            };
        }
    }
}
