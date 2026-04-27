using cod.application.Common.Extensions;
using cod.application.Dish.Common;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.PurchaseOrder.Common;
using cod.domain.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Dish.Commands.CreateDish
{
    public class CreateDishCommand : BaseCommand, ICreateDishCommand
    {
        public CreateDishCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(DishModel model)
        {
            this.ValidateModel(model);

            var dish = new domain.SalePoint.Dish
            {
                Name = model.Name,
                DishType = model.DishType,
                //SaleTypeDish = model.SaleTypeDish,
                //NoAddOns = model.NoAddOns,
                //FreeAddOnsQuantity = model.NoAddOns ? 0 : model.FreeAddOnsQuantity,
                //PriceWithCarnet = model.PriceWithCarnet,
                //PriceWithOutCarnet = model.PriceWithOutCarnet
            };
            _repository.InsertAsync(dish);
            await _repository.CommitAsync();
        }

        public void ValidateModel(DishModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Required));

            if(!model.DishType.IsValid())
                this._exceptionFactory.Add("dishType", this._exceptionFactory.GetMessage("Tipo Plato", ExceptionEnums.ExceptionCode.Required));

            /*if (!model.SaleTypeDish.IsValid())
                this._exceptionFactory.Add("saleTypeDish", this._exceptionFactory.GetMessage("Tipo Venta", ExceptionEnums.ExceptionCode.Required));*/

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "CAFETERIA", this._exceptionFactory.Exceptions);

            /*if ((model.SaleTypeDish == domain.Features.Enums.DomainEnums.SaleTypeDish.Both || model.SaleTypeDish == domain.Features.Enums.DomainEnums.SaleTypeDish.WithOutCarnet) && model.PriceWithOutCarnet <= 0)
                this._exceptionFactory.Add("priceWithOutCarnet", this._exceptionFactory.GetMessage("Precio sin Carnet", ExceptionEnums.ExceptionCode.Required));

            if ((model.SaleTypeDish == domain.Features.Enums.DomainEnums.SaleTypeDish.Both || model.SaleTypeDish == domain.Features.Enums.DomainEnums.SaleTypeDish.CarnetOnly) && model.PriceWithCarnet <= 0)
                this._exceptionFactory.Add("priceWithCarnet", this._exceptionFactory.GetMessage("Precio con Carnet", ExceptionEnums.ExceptionCode.Required));

            if (!model.NoAddOns && model.FreeAddOnsQuantity <= 0)
                this._exceptionFactory.Add("freeAddOnsQuantity", this._exceptionFactory.GetMessage("Max. Cantidad de Complementos Gratis", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "CAFETERIA", this._exceptionFactory.Exceptions);*/

        }
    }
}
