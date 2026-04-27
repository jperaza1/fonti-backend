using cod.application.Common.Extensions;
using cod.application.Dish.Common;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.PurchaseOrder.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Dish.Queries.GetDishes
{
    public class GetDishesQuery : BaseQuery, IGetDishesQuery
    {
        public GetDishesQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<DishModel>> Execute()
        {
            var baseQuery = _repository.Query<domain.SalePoint.Dish>().Select(f => new
            {
                f.Id,
                f.Name,
                f.DishType,
                f.IsActive
            });

            return await baseQuery.Select(x => new DishModel
            {
                Id = x.Id,
                DishTypeName = x.DishType.GetEnumDescription(),
                DishType = x.DishType,
                Name = x.Name,
                IsActive = x.IsActive
            }).ToListAsync();
        }
    }
}
