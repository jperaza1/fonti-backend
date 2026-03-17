using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Sale.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Sale.Queries.GetSale
{
    public class GetSaleQuery : BaseQuery, IGetSaleQuery
    {
        public GetSaleQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<SaleModel> Execute(int id)
        {
            var baseQuery = _repository.Query<domain.Operation.Sale>()
                .Where(f => f.Id == id)
                .Select(f => new
            {
                f.Id,
                f.CategoryId,
                Cafeteria = f.Cafeteria.Name,
                f.RegisterId,
                Register = f.RegisterId.HasValue ? f.Register.Name : string.Empty,
                f.CustomerId,
                Customer = f.CustomerId.HasValue ? f.Customer.Name : string.Empty,
                ServicePeriod = f.ServicePeriodId.HasValue ? f.ServicePeriod.Name : string.Empty,
                f.ServicePeriodId,
                f.SaleDate,
                f.SaleType,
                SaleTypeName = f.SaleType.GetEnumDescription(),
                f.CafeteriaId,
                Category = f.Category.Name,
                f.ProductTypeId,
                ProductType = f.ProductType.Name,
                f.Amount,
                f.IsActive
            });

            return await baseQuery.Select(f => new SaleModel
            {
                Id = f.Id,
                Amount = f.Amount,
                CafeteriaId = f.CafeteriaId,
                Cafeteria = f.Cafeteria,
                Register = f.Register,
                RegisterId = f.RegisterId,
                Category = f.Category,
                CategoryId = f.CategoryId,
                ProductTypeId = f.ProductTypeId,
                ProductType = f.ProductType,
                SaleDate = f.SaleDate,
                SaleType = f.SaleType,
                SaleTypeName = f.SaleTypeName,
                IsActive = f.IsActive,
                ServicePeriod = f.ServicePeriod,
                ServicePeriodId = f.ServicePeriodId,
                Customer = f.Customer,
                CustomerId = f.CustomerId
            }).FirstOrDefaultAsync();
        }
    }
}
