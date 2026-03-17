using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Main;
using cod.application.Product.Common;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Interfaces.Drawing.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Product.Queries.GetProducts
{
    public class GetProductsQuery : BaseQuery, IGetProductsQuery
    {
        public GetProductsQuery(
            IRepository repository
            ) : base(repository)
        {
            
        }

        public async Task<List<ProductModel>> Execute()
        {
            var baseQuery = _repository.Query<domain.Data.Product>().Select(f => new
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
                f.UnitCost,
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
                Measure = f.Measure,
                UnitCost = f.UnitCost,
            }).ToListAsync();
        }
    }
}
