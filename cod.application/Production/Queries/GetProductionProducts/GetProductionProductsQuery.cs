using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Production.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.Production.Queries.GetProductionProducts
{
    public interface IGetProductionProductsQuery
    {
        Task<List<ProductionProductModel>> Execute();
    }

    public class GetProductionProductsQuery : BaseQuery, IGetProductionProductsQuery
    {
        public GetProductionProductsQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<ProductionProductModel>> Execute()
        {
            return await _repository.QueryActive<domain.Data.ProductRecipe>()
                .Where(r => r.Product.IsActive)
                .OrderBy(r => r.Product.Name)
                .Select(r => new ProductionProductModel
                {
                    ProductId = r.ProductId,
                    Product = r.Product.Name
                })
                .ToListAsync();
        }
    }
}
