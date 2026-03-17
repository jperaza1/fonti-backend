using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Inventory.Common;
using cod.application.PurchaseOrder.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Inventory.Commands.UpdateInventory
{
    public class UpdateInventoryCommand : BaseCommand, IUpdateInventoryCommand
    {
        public UpdateInventoryCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(int id, InventoryModel model)
        {
            var inventory = await _repository.Query<domain.Operation.Inventory>().FirstOrDefaultAsync(f => f.Id == id);

            if (inventory == null)
                throw new ApplicationException("El inventario no existe", "ORDEN COMPRA");

            inventory.Total = Math.Round(model.Details.Sum(f => f.Total), 2);
            inventory.RegisterId = model.UseRegister ? model.RegisterId : null;
            inventory.UseInInventory = model.UseRegister;
            inventory.InventoryType = model.InventoryType;

            _repository.UpdateSync(inventory);

            var details = await _repository.Query<domain.Operation.InventoryDetail>().Where(f => f.InventoryId == id).ToListAsync();

            var detailsToRemove = details.Where(detail => model.Details.All(member => member.Id != detail.Id)).ToList();

            foreach (var item in detailsToRemove)
            {
                _repository.DeleteSync(item);
            }

            var detailToAdd = model.Details.Where(dd => dd.Id <= 0).ToList();

            foreach (var item in detailToAdd)
            {
                _repository.InsertAsync(new domain.Operation.InventoryDetail
                {
                    InventoryId = id,
                    ProductId = item.ProductId,
                    QuantityPack = item.QuantityPack,
                    UnitQuantity = item.UnitQuantity,
                    UnitCost = item.UnitCost,
                    SubTotal = Math.Round(item.SubTotal, 2),
                    Tax = Math.Round(item.Tax, 2),
                    TaxPercentage = item.TaxPercentage,
                    Discount = Math.Round(item.Discount, 2),
                    DiscountPercentage = item.DiscountPercentage,
                    Cost = item.Cost,
                    Total = Math.Round(item.Total, 2)
                });
            }

            var detailToUpdate = model.Details.Where(dd => dd.Id > 0).ToList();

            foreach (var item in detailToUpdate)
            {
                await _repository.UpdateAsync<domain.Operation.InventoryDetail>(
                    registry => registry.Id == item.Id,
                    registry => new domain.Operation.InventoryDetail
                    {

                        QuantityPack = item.QuantityPack,
                        UnitQuantity = item.UnitQuantity,
                        UnitCost = item.UnitCost,
                        SubTotal = Math.Round(item.SubTotal, 2),
                        Tax = Math.Round(item.Tax, 2),
                        TaxPercentage = item.TaxPercentage,
                        Discount = Math.Round(item.Discount, 2),
                        DiscountPercentage = item.DiscountPercentage,
                        Cost = item.Cost,
                        Total = Math.Round(item.Total, 2)
                    });
            }

            await _repository.CommitAsync();
        }

        
    }
}
