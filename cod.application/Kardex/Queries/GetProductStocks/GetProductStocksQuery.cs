using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Inventory.Services;
using cod.application.Kardex.Common;
using cod.domain.Features.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Kardex.Queries.GetProductStocks
{
    public class GetProductStocksQuery : BaseQuery, IGetProductStocksQuery
    {
        private readonly IWarehouseDirectory _warehouses;

        public GetProductStocksQuery(IRepository repository, IWarehouseDirectory warehouses) : base(repository)
        {
            _warehouses = warehouses;
        }

        public async Task<List<ProductStockModel>> Execute(int? warehouseId)
        {
            if (warehouseId.HasValue && warehouseId.Value > 0)
                return await ByWarehouse(warehouseId.Value);

            return await All();
        }

        private async Task<List<ProductStockModel>> ByWarehouse(int warehouseId)
        {
            var warehouse = await _warehouses.Get(warehouseId);
            if (warehouse == null)
                throw new ApplicationException("La bodega no existe", "INVENTARIO");

            var quantities = await _repository.Query<domain.Operation.ProductStock>()
                .Where(stock => stock.WarehouseId == warehouseId)
                .Select(stock => new { stock.ProductId, stock.Quantity })
                .ToListAsync();

            var byProduct = quantities.ToDictionary(stock => stock.ProductId, stock => stock.Quantity);
            var products = await Products();

            return products
                .Where(product => _warehouses.Allows(warehouse.Kind, product.ProductType))
                .Select(product => Map(product, byProduct.ContainsKey(product.Id) ? byProduct[product.Id] : 0, warehouse.Id, warehouse.Name))
                .OrderBy(product => product.Product)
                .ToList();
        }

        private async Task<List<ProductStockModel>> All()
        {
            var quantities = await _repository.Query<domain.Operation.ProductStock>()
                .Select(stock => new { stock.ProductId, stock.Quantity })
                .ToListAsync();

            var byProduct = quantities
                .GroupBy(stock => stock.ProductId)
                .ToDictionary(group => group.Key, group => group.Sum(stock => stock.Quantity));

            var products = await Products();

            return products
                .Select(product => Map(product, byProduct.ContainsKey(product.Id) ? byProduct[product.Id] : 0, null, null))
                .OrderBy(product => product.Product)
                .ToList();
        }

        private async Task<List<ProductRow>> Products()
        {
            return await _repository.QueryActive<domain.Data.Product>()
                .Select(product => new ProductRow
                {
                    Id = product.Id,
                    Name = product.Name,
                    ProductType = product.ProductType,
                    UnitOfMeasure = product.UnitOfMeasure.Name,
                    Cost = product.Cost
                })
                .ToListAsync();
        }

        private static ProductStockModel Map(ProductRow product, decimal quantity, int? warehouseId, string warehouse)
        {
            return new ProductStockModel
            {
                ProductId = product.Id,
                Product = product.Name,
                ProductType = product.ProductType,
                ProductTypeName = product.ProductType.GetEnumDescription(),
                UnitOfMeasure = product.UnitOfMeasure,
                Cost = product.Cost,
                Quantity = quantity,
                WarehouseId = warehouseId,
                Warehouse = warehouse
            };
        }

        private class ProductRow
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public ProductType ProductType { get; set; }
            public string UnitOfMeasure { get; set; }
            public decimal? Cost { get; set; }
        }
    }
}
