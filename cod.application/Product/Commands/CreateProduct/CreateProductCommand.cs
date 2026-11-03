using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Product.Common;
using cod.domain.Features.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Product.Commands.CreateProduct
{
    public class CreateProductCommand : BaseCommand, ICreateProductCommand
    {
        public CreateProductCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(ProductModel model)
        {
            await ValidateModel(model);

            var isRawMaterial = model.ProductType == ProductType.RawMaterial;
            var newProduct = new domain.Data.Product
            {
                Name = model.Name,
                ProductType = model.ProductType,
                Tax = model.Tax,
                Cost = isRawMaterial ? ActiveVendors(model).First().UnitCost : null,
                UnitOfMeasureId = isRawMaterial ? model.UnitOfMeasureId : null,
                ProductVendors = isRawMaterial ? BuildVendors(model) : null
            };

            if (newProduct.ProductVendors != null)
            {
                foreach (var vendor in newProduct.ProductVendors)
                    _repository.SetAuthValuesToCreate(vendor);
            }

            _repository.InsertAsync(newProduct);

            await _repository.CommitAsync();
        }

        public async Task ValidateModel(ProductModel model)
        {
            if (model.Name.NullOrEmpty())
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Required));

            if (!model.ProductType.IsValid())
                this._exceptionFactory.Add("productType", this._exceptionFactory.GetMessage("Tipo de Producto", ExceptionEnums.ExceptionCode.Required));

            ValidateRawMaterial(model);

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "PRODUCTO", this._exceptionFactory.Exceptions);

            bool duplicateName = await _repository
                .AnyAsync<domain.Data.Product>(f => f.Name == model.Name && f.ProductType != model.ProductType);

            if (duplicateName)
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Duplicate));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "PRODUCTO", this._exceptionFactory.Exceptions);

            await ValidateRawMaterialReferences(model);
        }

        private void ValidateRawMaterial(ProductModel model)
        {
            if (model.ProductType != ProductType.RawMaterial)
                return;

            if (!model.UnitOfMeasureId.HasValue || model.UnitOfMeasureId <= 0)
                this._exceptionFactory.Add("unitOfMeasureId", this._exceptionFactory.GetMessage("Unidad de medida", ExceptionEnums.ExceptionCode.Required));

            var vendors = ActiveVendors(model);
            if (!vendors.Any())
                this._exceptionFactory.Add("details", this._exceptionFactory.GetMessage("Proveedores", ExceptionEnums.ExceptionCode.Required));

            foreach (var item in vendors)
                ValidateVendorLine(item);

            var duplicated = vendors
                .GroupBy(item => new { item.VendorId, item.UnitOfMeasureId })
                .Any(group => group.Count() > 1);

            if (duplicated)
                this._exceptionFactory.Add("details", "El proveedor ya tiene esa unidad de medida");
        }

        private void ValidateVendorLine(ProductVendorModel item)
        {
            if (item.VendorId <= 0)
                this._exceptionFactory.Add("details", this._exceptionFactory.GetMessage("Proveedor", ExceptionEnums.ExceptionCode.Required));

            if (item.UnitOfMeasureId <= 0)
                this._exceptionFactory.Add("details", this._exceptionFactory.GetMessage("Unidad de medida", ExceptionEnums.ExceptionCode.Required));

            if (item.Unit <= 0)
                this._exceptionFactory.Add("details", this._exceptionFactory.GetMessage("Unidades por medida", ExceptionEnums.ExceptionCode.Required));

            if (item.Cost <= 0)
                this._exceptionFactory.Add("details", this._exceptionFactory.GetMessage("Costo", ExceptionEnums.ExceptionCode.Required));

            if (item.UnitCost <= 0)
                this._exceptionFactory.Add("details", this._exceptionFactory.GetMessage("Costo unitario", ExceptionEnums.ExceptionCode.Required));

            if (item.Discount < 0)
                this._exceptionFactory.Add("details", "El descuento no puede ser negativo");
        }

        private async Task ValidateRawMaterialReferences(ProductModel model)
        {
            if (model.ProductType != ProductType.RawMaterial)
                return;

            bool unitExists = await _repository.AnyActive<domain.Data.UnitOfMeasure>(f => f.Id == model.UnitOfMeasureId);
            if (!unitExists)
                this._exceptionFactory.Add("unitOfMeasureId", "La unidad de medida no existe");

            var vendors = ActiveVendors(model);
            var vendorIds = vendors.Select(item => item.VendorId).Distinct().ToList();
            var measureIds = vendors.Select(item => item.UnitOfMeasureId).Distinct().ToList();

            var existingVendors = await _repository.QueryActive<domain.Data.Vendor>()
                .Where(vendor => vendorIds.Contains(vendor.Id))
                .Select(vendor => vendor.Id)
                .ToListAsync();

            var existingMeasures = await _repository.QueryActive<domain.Data.UnitOfMeasure>()
                .Where(measure => measureIds.Contains(measure.Id))
                .Select(measure => measure.Id)
                .ToListAsync();

            if (vendorIds.Any(id => !existingVendors.Contains(id)))
                this._exceptionFactory.Add("details", "El proveedor no existe");

            if (measureIds.Any(id => !existingMeasures.Contains(id)))
                this._exceptionFactory.Add("details", "La unidad de medida del proveedor no existe");

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "PRODUCTO", this._exceptionFactory.Exceptions);
        }

        private static List<ProductVendorModel> ActiveVendors(ProductModel model)
        {
            return (model.ProductVendors ?? new List<ProductVendorModel>())
                .Where(item => item.IsActive)
                .ToList();
        }

        private static List<domain.Data.ProductVendor> BuildVendors(ProductModel model)
        {
            return ActiveVendors(model).Select(item => new domain.Data.ProductVendor
            {
                VendorId = item.VendorId,
                UnitOfMeasureId = item.UnitOfMeasureId,
                Unit = item.Unit,
                Cost = item.Cost,
                UnitCost = item.UnitCost,
                Discount = item.Discount
            }).ToList();
        }
    }
}
