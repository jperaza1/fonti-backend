using cod.application.Cafeteria.Commands;
using cod.application.Features.Base;
using cod.application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Cafeteria.Queries.GetCafeterias
{
    public class GetCafeteriasQuery : BaseQuery, IGetCafeteriasQuery
    {

        public GetCafeteriasQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<CafeteriaModel>> Execute()
        {
            return await _repository.Query<domain.Data.Cafeteria>().Select(f => new CafeteriaModel
            {
                Id = f.Id,
                Name = f.Name,
                Code = f.Code,
                PurchaseOrderCorelative = f.PurchaseOrderCorelative,
                IsActive = f.IsActive,
            }).ToListAsync();
        }
    }
}
