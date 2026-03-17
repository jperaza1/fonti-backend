using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Product.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Product.Queries.GetProductsBySpecialService
{
    public class GetProductsBySpecialServiceQuery : BaseQuery, IGetProductsBySpecialServiceQuery
    {
        public GetProductsBySpecialServiceQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<ProductModel>> Execute()
        {
            var baseQuery = _repository.QueryActive<domain.Data.Product>()
                .Where(f => f.SpecialServiceUse)
                .Select(f => new
                {
                    f.Id,
                    f.Name,
                    f.Unit,
                    f.Cost,
                    f.Tax,
                    f.Discount,
                    f.IsActive,
                    f.VendorId,
                    Vendor = f.Vendor.Name,
                    f.MeasureId,
                    Measure = f.Measure.Name,
                });

            return await baseQuery.Select(f => new ProductModel
            {
                Id = f.Id,
                Name = f.Name,
                Unit = f.Unit,
                Cost = f.Cost,
                Tax = f.Tax,
                Discount = f.Discount,
                IsActive = f.IsActive,
                VendorId = f.VendorId,
                MeasureId = f.MeasureId,
                Vendor = f.Vendor,
                Measure = f.Measure
            }).ToListAsync();
        }
    }
}
