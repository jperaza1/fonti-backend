using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Register.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Register.Queries.GetActiveRegisters
{
    public class GetActiveRegistersQuery : BaseQuery, IGetActiveRegistersQuery
    {
        public GetActiveRegistersQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<RegisterModel>> Execute()
        {
            return await _repository.QueryActive<domain.Data.Register>().Select(f => new RegisterModel
            {
                Id = f.Id,
                Name = f.Name,
                Cafeteria = f.Cafeteria.Name,
                CafeteriaId = f.CafeteriaId,
                IsActive = f.IsActive,
            }).ToListAsync();
        }
    }
}
