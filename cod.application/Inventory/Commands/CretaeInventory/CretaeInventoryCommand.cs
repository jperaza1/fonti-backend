using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Inventory.Common;
using cod.application.PurchaseOrder.Common;
using cod.domain.Features;
using cod.domain.Features.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Inventory.Commands.CretaeInventory
{
    public class CretaeInventoryCommand : BaseCommand, ICretaeInventoryCommand
    {
        private readonly AuthService _authService;
        public CretaeInventoryCommand(
            IRepository repository, 
            IExceptionFactory exceptionFactory,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task<int> Execute(InventoryModel model)
        {
            this.ValidateModel(model);
            var cafeteria = await _repository.QueryActive<domain.Data.Cafeteria>().Where(f => f.Id == model.CafeteriaId).FirstOrDefaultAsync();

            if (cafeteria == null) throw new ApplicationException("Cafeteria no existe", "INVENTARIO");
            var reference = $"{cafeteria.Code}-INV-{cafeteria.InventoryCorelative.ToString("D5")}";
            cafeteria.InventoryCorelative++;

            var inventory = new domain.Operation.Inventory
            {
                CafeteriaId = model.CafeteriaId,
                Date = model.Date,
                Reference = reference,
                Total = model.Details.Sum(f => f.Total),
                RegisterId = model.RegisterId,
                UseInInventory = model.UseRegister,
                InventoryType = model.InventoryType,
                InventoryDetails = GenerateDetails(model)
            };

            _repository.InsertAsync(inventory);
            _repository.UpdateSync(cafeteria);
            await _repository.CommitAsync();

            return inventory.Id;
        }

        public List<domain.Operation.InventoryDetail> GenerateDetails(InventoryModel model)
        {
            var lst = new List<domain.Operation.InventoryDetail>();
            foreach (var item in model.Details)
            {
                var detail = new domain.Operation.InventoryDetail
                {
                    ProductId = item.ProductId,
                    QuantityPack = item.QuantityPack,
                    UnitQuantity = item.UnitQuantity,
                    SubTotal = Math.Round(item.SubTotal, 2),
                    Tax = Math.Round(item.Tax, 2),
                    TaxPercentage = item.TaxPercentage,
                    Discount = Math.Round(item.Discount, 2),
                    DiscountPercentage = item.DiscountPercentage,
                    Cost = item.Cost,
                    UnitCost = item.UnitCost,
                    Total = Math.Round(item.Total, 2),
                    IsActive = true,
                    CreationTime = Clock.Now,
                    CreatorUser = _authService.User,
                };
                lst.Add(detail);
            }
            return lst;
        }

        public void ValidateModel(InventoryModel model)
        {

            if (model.CafeteriaId <= 0)
                this._exceptionFactory.Add("cafeteria", this._exceptionFactory.GetMessage("Cafeteria", ExceptionEnums.ExceptionCode.Required));

            if (model.Date == DateTime.MinValue)
                this._exceptionFactory.Add("date", this._exceptionFactory.GetMessage("Fecha", ExceptionEnums.ExceptionCode.Required));

            if (model.UseRegister && !model.RegisterId.HasValue)
            {
                this._exceptionFactory.Add("register", this._exceptionFactory.GetMessage("Caja", ExceptionEnums.ExceptionCode.Required));
            }


            if (model.Details.Count == 0)
                this._exceptionFactory.Add("details", this._exceptionFactory.GetMessage("Producto Requerido", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "CAFETERIA", this._exceptionFactory.Exceptions);
        }
    }
}
