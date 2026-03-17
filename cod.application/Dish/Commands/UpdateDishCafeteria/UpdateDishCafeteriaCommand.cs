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

namespace cod.application.Dish.Commands.UpdateDishCafeteria
{
    public class UpdateDishCafeteriaCommand : BaseCommand, IUpdateDishCafeteriaCommand
    {
        public UpdateDishCafeteriaCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(DishCafeteriaModel model, int id)
        {
            ValidateModel(model);

            var dish = await _repository.Query<domain.SalePoint.DishCafeteria>().FirstOrDefaultAsync(f => f.Id == id);

            if (dish == null)
                throw new ApplicationException("El plato no existe", "PLATO");

            dish.SaleTypeDish = model.SaleTypeDish;
            dish.NoAddOns = model.NoAddOns;
            dish.FreeAddOnsQuantity = model.FreeAddOnsQuantity;
            dish.PriceWithCarnet = model.PriceWithCarnet;
            dish.PriceWithOutCarnet = model.PriceWithOutCarnet;

            _repository.UpdateSync(dish);
            await _repository.CommitAsync();
        }

        public void ValidateModel(DishCafeteriaModel model)
        {
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

        }
    }
}
