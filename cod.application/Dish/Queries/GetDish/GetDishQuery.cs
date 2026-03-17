using cod.application.Common.Extensions;
using cod.application.Dish.Common;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.PurchaseOrder.Common;
using cod.domain.Operation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Dish.Queries.GetDish
{
    public class GetDishQuery : BaseQuery, IGetDishQuery
    {
        public GetDishQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<DishModel> Execute(int id)
        {
            var baseQuery = _repository.Query<domain.SalePoint.Dish>()
                .Where(x => x.Id == id)
                .Select(f => new
            {
                f.Id,
                f.Name,
                f.DishType,
                f.IsActive
            });

            var dish = await baseQuery.FirstOrDefaultAsync();

            if (dish == null)
                throw new ApplicationException("El Plato no existe", "PLATOS");

            return new DishModel
            {
                Id = dish.Id,
                Name = dish.Name,
                DishType = dish.DishType,
                DishTypeName = dish.DishType.GetEnumDescription(),
                IsActive = dish.IsActive
            };
        }
    }
}
