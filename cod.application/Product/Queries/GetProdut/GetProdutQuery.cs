using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Product.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Product.Queries.GetProdut
{
    public class GetProdutQuery : BaseQuery, IGetProdutQuery
    {
        public GetProdutQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<ProductModel> Execute(int id)
        {
            var baseQuery = _repository.Query<domain.Data.Product>().Where(f => f.Id == id).Select(f => new
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
                f.SpecialServiceUse,
            });

            var product = await baseQuery.FirstOrDefaultAsync();

            if (product == null)
                throw new ApplicationException("Producto no existe", "PRODUCTO");

            return new ProductModel
            {
                Id = product.Id,
                Name = product.Name,
                Unit = product.Unit,
                Cost = product.Cost,
                Tax = product.Tax,
                Discount = product.Discount,
                IsActive = product.IsActive,
                VendorId = product.VendorId,
                MeasureId = product.MeasureId,
                Vendor = product.Vendor,
                Measure = product.Measure,
                SpecialServiceUse = product.SpecialServiceUse,
                UnitCost = product.UnitCost,
            };
        }
    }
}
