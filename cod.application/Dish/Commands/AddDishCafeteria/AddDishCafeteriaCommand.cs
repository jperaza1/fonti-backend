using cod.application.Common.Extensions;
using cod.application.Dish.Common;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Dish.Commands.AddDishCafeteria
{
    public class AddDishCafeteriaCommand : BaseCommand, IAddDishCafeteriaCommand
    {
        public AddDishCafeteriaCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(DishCafeteriaModel model)
        {
            await this.ValidateModel(model);

            var dish = new domain.SalePoint.DishCafeteria
            {
                CafeteriaId = model.CafeteriaId,
                DishId = model.DishId,
                SaleTypeDish = model.SaleTypeDish,
                NoAddOns = model.NoAddOns,
                FreeAddOnsQuantity = model.NoAddOns ? 0 : model.FreeAddOnsQuantity,
                PriceWithCarnet = model.PriceWithCarnet,
                PriceWithOutCarnet = model.PriceWithOutCarnet
            };
            _repository.InsertAsync(dish);
            await _repository.CommitAsync();
        }

        public async Task ValidateModel(DishCafeteriaModel model)
        {
            if(model.CafeteriaId <= 0)
                this._exceptionFactory.Add("cafeteria", this._exceptionFactory.GetMessage("Cafeteria", ExceptionEnums.ExceptionCode.Required));

            if (model.DishId <= 0)
                this._exceptionFactory.Add("dish", this._exceptionFactory.GetMessage("Plato", ExceptionEnums.ExceptionCode.Required));

            if (!model.SaleTypeDish.IsValid())
                this._exceptionFactory.Add("saleTypeDish", this._exceptionFactory.GetMessage("Tipo Venta", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "CAFETERIA", this._exceptionFactory.Exceptions);

            if ((model.SaleTypeDish == domain.Features.Enums.DomainEnums.SaleTypeDish.Both || model.SaleTypeDish == domain.Features.Enums.DomainEnums.SaleTypeDish.WithOutCarnet) && model.PriceWithOutCarnet <= 0)
                this._exceptionFactory.Add("priceWithOutCarnet", this._exceptionFactory.GetMessage("Precio sin Carnet", ExceptionEnums.ExceptionCode.Required));

            if ((model.SaleTypeDish == domain.Features.Enums.DomainEnums.SaleTypeDish.Both || model.SaleTypeDish == domain.Features.Enums.DomainEnums.SaleTypeDish.CarnetOnly) && model.PriceWithCarnet <= 0)
                this._exceptionFactory.Add("priceWithCarnet", this._exceptionFactory.GetMessage("Precio con Carnet", ExceptionEnums.ExceptionCode.Required));

            if (!model.NoAddOns && model.FreeAddOnsQuantity <= 0)
                this._exceptionFactory.Add("freeAddOnsQuantity", this._exceptionFactory.GetMessage("Max. Cantidad de Complementos Gratis", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "CAFETERIA", this._exceptionFactory.Exceptions);

            var existDishCafeteria = await _repository.Query<domain.SalePoint.DishCafeteria>()
                .AnyAsync(f => f.CafeteriaId == model.CafeteriaId && f.DishId == model.DishId);

            if(existDishCafeteria)
                throw new ApplicationException("EL PLATO YA SE AGREGO A LA CAFETERIA", "CAFETERIA");

        }
    }
}
