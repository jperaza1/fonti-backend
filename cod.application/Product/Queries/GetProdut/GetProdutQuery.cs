using cod.application.Common.Extensions;
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
            var product = await _repository.Query<domain.Data.Product>().Where(f => f.Id == id).Select(f => new
            {
                f.Id,
                f.Name,
                f.ProductType,
                f.Tax,
                f.Cost,
                f.UnitOfMeasureId,
                UnitOfMeasureName = f.UnitOfMeasure.Name,
                f.IsActive,
                Vendors = f.ProductVendors.Select(g => new
                {
                    g.Id,
                    g.VendorId,
                    VendorName = g.Vendor.Name,
                    g.ProductId,
                    g.UnitOfMeasureId,
                    UnitOfMeasureName = g.UnitOfMeasure.Name,
                    g.Unit,
                    g.Cost,
                    g.UnitCost,
                    g.Discount,
                    g.IsActive
                }).ToList()
            }).FirstOrDefaultAsync();

            if (product == null)
                throw new ApplicationException("Producto no existe", "PRODUCTO");

            return new ProductModel
            {
                Id = product.Id,
                Name = product.Name,
                ProductType = product.ProductType,
                ProductTypeName = product.ProductType.GetEnumDescription(),
                Tax = product.Tax,
                Cost = product.Cost,
                UnitOfMeasureId = product.UnitOfMeasureId,
                UnitOfMeasureName = product.UnitOfMeasureName,
                IsActive = product.IsActive,
                ProductVendors = product.Vendors
                    .OrderBy(vendor => vendor.Id)
                    .Select(vendor => new ProductVendorModel
                    {
                        Id = vendor.Id,
                        VendorId = vendor.VendorId,
                        VendorName = vendor.VendorName,
                        ProductId = vendor.ProductId,
                        UnitOfMeasureId = vendor.UnitOfMeasureId,
                        UnitOfMeasureName = vendor.UnitOfMeasureName,
                        Unit = vendor.Unit,
                        Cost = vendor.Cost,
                        UnitCost = vendor.UnitCost,
                        Discount = vendor.Discount,
                        IsActive = vendor.IsActive
                    }).ToList()
            };
        }
    }
}
