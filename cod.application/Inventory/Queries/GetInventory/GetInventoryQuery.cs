using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Interfaces;
using cod.application.Inventory.Common;
using cod.application.PurchaseOrder.Common;
using cod.domain.Operation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Inventory.Queries.GetInventory
{
    public class GetInventoryQuery : BaseQuery, IGetInventoryQuery
    {
        public GetInventoryQuery(IRepository repository) : base(repository)
        {
        }

        public async Task<InventoryModel> Execute(int id)
        {
            var baseQuery = _repository.Query<domain.Operation.Inventory>()
                .Where(f => f.Id == id)
                .Select(f => new
                {
                    f.Id,
                    f.CafeteriaId,
                    Cafeteria = f.Cafeteria.Name,
                    f.RegisterId,
                    Register = f.RegisterId.HasValue ? f.Register.Name : string.Empty,
                    f.InventoryType,
                    f.UseInInventory,
                    f.Total,
                    f.Date,
                    f.Reference,
                    f.ReasonCanceled,
                    f.CanceledAt,
                    f.CanceledUser,
                    InventoryDetails = f.InventoryDetails.Select(x => new
                    {
                        x.Id,
                        x.InventoryId,
                        x.ProductId,
                        Product = $"{x.Product.Name} ({x.Product.Measure.Name}) ({x.Product.Vendor.Name})",
                        x.Cost,
                        x.UnitCost,
                        x.QuantityPack,
                        x.UnitQuantity,
                        x.Discount,
                        x.DiscountPercentage,
                        x.Tax,
                        x.TaxPercentage,
                        x.SubTotal,
                        x.Total,
                    }).ToList()
                });

            var inventory = await baseQuery.FirstOrDefaultAsync();

            if (inventory == null)
                throw new ApplicationException("EL Inventario no existe", "INVENTARIO");

            return new InventoryModel
            {
                Id = inventory.Id,
                CafeteriaId = inventory.CafeteriaId,
                Cafeteria = inventory.Cafeteria,
                Total = inventory.Total,
                Date = inventory.Date.Date,
                InventoryType = inventory.InventoryType,
                InventoryTypeName = inventory.InventoryType.GetEnumDescription(),
                Reference = inventory.Reference,
                ReasonCanceled = inventory.ReasonCanceled,
                CanceledUser = inventory.CanceledUser,
                CanceledAt = inventory.CanceledAt,
                RegisterId = inventory.RegisterId,
                Register = inventory.Register,
                UseRegister = inventory.UseInInventory,
                Details = inventory.InventoryDetails.Select(f => new InventoryDetailModel
                {
                    Id = f.Id,
                    InventoryId = f.InventoryId,
                    Cost = f.Cost,
                    QuantityPack = f.QuantityPack,
                    UnitCost = f.UnitCost,
                    UnitQuantity = f.UnitQuantity,
                    Tax = f.Tax,
                    TaxPercentage = f.TaxPercentage,
                    SubTotal = f.SubTotal,
                    Total = f.Total,
                    Product = f.Product,
                    ProductId = f.ProductId,
                    Discount = f.Discount,
                    DiscountPercentage = f.DiscountPercentage,
                }).ToList()
            };
        }
    }
}
