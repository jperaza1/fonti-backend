using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Kardex.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cod.application.Kardex.Queries.GetProductStocks
{
    public class GetProductStocksQuery : BaseQuery, IGetProductStocksQuery
    {
        public GetProductStocksQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<List<ProductStockModel>> Execute()
        {
            var stocks = await _repository.Query<domain.Operation.ProductStock>()
                .Select(f => new
                {
                    f.ProductId,
                    f.Quantity
                })
                .ToListAsync();

            var quantities = stocks.ToDictionary(f => f.ProductId, f => f.Quantity);

            var products = await _repository.QueryActive<domain.Data.Product>()
                .Select(f => new
                {
                    f.Id,
                    f.Name,
                    f.ProductType,
                    UnitOfMeasure = f.UnitOfMeasure.Name,
                    f.Cost
                })
                .ToListAsync();

            return products
                .Select(f => new ProductStockModel
                {
                    ProductId = f.Id,
                    Product = f.Name,
                    ProductType = f.ProductType,
                    ProductTypeName = f.ProductType.GetEnumDescription(),
                    UnitOfMeasure = f.UnitOfMeasure,
                    Cost = f.Cost,
                    Quantity = quantities.ContainsKey(f.Id) ? quantities[f.Id] : 0
                })
                .OrderBy(f => f.Product)
                .ToList();
        }
    }
}
