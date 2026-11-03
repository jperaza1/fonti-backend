using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Product.Common;
using cod.domain.Features.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.Product.Queries.GetProductsByVendor
{
    public class GetProductsByVendorQuery : BaseQuery, IGetProductsByVendorQuery
    {
        public GetProductsByVendorQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<VendorProductModel>> Execute(int vendorId)
        {
            return await _repository.QueryActive<domain.Data.ProductVendor>()
                .Where(item => item.VendorId == vendorId
                    && item.Product.IsActive
                    && item.Product.ProductType == ProductType.RawMaterial)
                .OrderBy(item => item.Product.Name)
                .Select(item => new VendorProductModel
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Name = item.Product.Name + " (" + item.UnitOfMeasure.Name + ")",
                    Cost = item.Cost,
                    Discount = item.Discount,
                    TaxRate = item.Product.Tax ? 0.15m : 0m,
                    Unit = item.Unit
                })
                .ToListAsync();
        }
    }
}
