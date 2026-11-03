using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.ProductRecipe.Common;
using cod.domain.Features.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.ProductRecipe.Commands.SaveProductRecipe
{
    public class SaveProductRecipeCommand : BaseCommand, ISaveProductRecipeCommand
    {
        public SaveProductRecipeCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(ProductRecipeModel model)
        {
            await ValidateModel(model);

            var recipe = await _repository.WhereAsync<domain.Data.ProductRecipe>(f => f.ProductId == model.ProductId && f.IsActive)
                .Include(f => f.Details)
                .FirstOrDefaultAsync();

            if (recipe == null)
            {
                recipe = new domain.Data.ProductRecipe
                {
                    ProductId = model.ProductId,
                    UnitsPerPackage = model.UnitsPerPackage,
                    Details = model.Details.Select(CreateDetail).ToList()
                };

                foreach (var detail in recipe.Details)
                    _repository.SetAuthValuesToCreate(detail);

                _repository.InsertAsync(recipe);
            }
            else
            {
                recipe.UnitsPerPackage = model.UnitsPerPackage;

                foreach (var detail in recipe.Details.Where(f => f.IsActive))
                    _repository.DisableSync(detail);

                foreach (var detailModel in model.Details)
                {
                    var detail = CreateDetail(detailModel);
                    detail.ProductRecipeId = recipe.Id;
                    _repository.InsertAsync(detail);
                }

                _repository.UpdateSync(recipe);
            }

            await _repository.CommitAsync();
        }

        private static domain.Data.ProductRecipeDetail CreateDetail(ProductRecipeDetailModel model)
        {
            return new domain.Data.ProductRecipeDetail
            {
                RawMaterialProductId = model.RawMaterialProductId,
                Quantity = model.Quantity,
                ProducedUnits = model.PerPackage ? 0 : model.ProducedUnits,
                PerPackage = model.PerPackage
            };
        }

        private async Task ValidateModel(ProductRecipeModel model)
        {
            if (model.ProductId <= 0)
                _exceptionFactory.Add("productId", _exceptionFactory.GetMessage("Producto", ExceptionEnums.ExceptionCode.Required));

            if (model.UnitsPerPackage <= 0)
                _exceptionFactory.Add("unitsPerPackage", "Las unidades por paquete deben ser mayor a cero");

            if (model.Details == null || !model.Details.Any())
                _exceptionFactory.Add("details", "La receta debe tener al menos un insumo");

            if (!_exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "RECETA", _exceptionFactory.Exceptions);

            var product = await _repository.QueryActive<domain.Data.Product>()
                .Where(f => f.Id == model.ProductId)
                .Select(f => new { f.Id, f.ProductType })
                .FirstOrDefaultAsync();

            if (product == null)
                _exceptionFactory.Add("productId", "El producto no existe");
            else if (product.ProductType != ProductType.Sale)
                _exceptionFactory.Add("productId", "La receta solo se asigna a un producto de venta");

            var rawMaterialIds = model.Details.Select(f => f.RawMaterialProductId).ToList();
            if (rawMaterialIds.Count != rawMaterialIds.Distinct().Count())
                _exceptionFactory.Add("details", "La receta no puede repetir el mismo insumo");

            var rawMaterials = await _repository.QueryActive<domain.Data.Product>()
                .Where(f => rawMaterialIds.Contains(f.Id))
                .Select(f => new { f.Id, f.ProductType })
                .ToListAsync();

            foreach (var detail in model.Details)
            {
                if (detail.Quantity <= 0)
                    _exceptionFactory.Add("quantity", "La cantidad del insumo debe ser mayor a cero");

                if (!detail.PerPackage && detail.ProducedUnits <= 0)
                    _exceptionFactory.Add("producedUnits", "Las unidades que produce el insumo deben ser mayor a cero");

                var rawMaterial = rawMaterials.FirstOrDefault(f => f.Id == detail.RawMaterialProductId);
                if (rawMaterial == null)
                    _exceptionFactory.Add("rawMaterialProductId", "El insumo no existe");
                else if (rawMaterial.ProductType != ProductType.RawMaterial)
                    _exceptionFactory.Add("rawMaterialProductId", "El insumo debe ser materia prima");
            }

            if (!_exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "RECETA", _exceptionFactory.Exceptions);
        }
    }
}
