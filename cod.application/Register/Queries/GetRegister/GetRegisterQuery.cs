using cod.application.Cafeteria.Commands;
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

namespace cod.application.Register.Queries.GetRegister
{
    public class GetRegisterQuery : BaseQuery, IGetRegisterQuery
    {
        public GetRegisterQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<RegisterModel> Execute(int id)
        {
            var register = await _repository.AllAsync<domain.Data.Register>().Where(f => f.Id == id).FirstOrDefaultAsync();

            if (register == null)
                throw new ApplicationException("Caja no existe", "CAJA");

            return new RegisterModel
            {
                Id = register.Id,
                Name = register.Name,
                Cafeteria = register.Cafeteria.Name,
                CafeteriaId = register.CafeteriaId,
                IsActive = register.IsActive,
                UseInInventory = register.UseInInventory,
            };
        }
    }
}
