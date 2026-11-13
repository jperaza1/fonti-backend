using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Route.Common;
using cod.domain.Features;
using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Route.Commands.CreateRoute
{
    public class CreateRouteCommand : BaseCommand, ICreateRouteCommand
    {
        private readonly AuthService _authService;
        public CreateRouteCommand(
            IRepository repository, 
            IExceptionFactory exceptionFactory,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task Execute(RouteModel model)
        {
            await ValidateModel(model);

            var newRoute = new domain.Data.Route
            {
                Name = model.Name,
                Vendor = model.Vendor,
                Order = model.Order,
                DeliversRegalia = model.DeliversRegalia,
                GivesCredit = model.GivesCredit,
                ProductRoutePrices = GenerateDetails(model)
            };

            _repository.InsertAsync(newRoute);

            await _repository.CommitAsync();
        }

        public List<domain.Data.ProductRoutePrice> GenerateDetails(RouteModel model)
        {
            var lst = new List<domain.Data.ProductRoutePrice>();
            foreach (var item in model.ProductRoutePrices)
            {
                var detail = new domain.Data.ProductRoutePrice
                {
                    ProductId = item.ProductId,
                    Price = item.Price,
                    CreditPrice = PositiveCreditPrice(item.CreditPrice),
                    IsActive = true,
                    CreationTime = Clock.Now,
                    CreatorUser = _authService.User,
                };
                lst.Add(detail);
            }
            return lst;
        }

        public async Task ValidateModel(RouteModel model)
        {
            if (model.Name.NullOrEmpty())
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Required));

            ValidatePrices(model);

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "RUTA", this._exceptionFactory.Exceptions);

            bool duplicateName = await _repository
                .AnyAsync<domain.Data.Route>(f => f.Name == model.Name);

            if (duplicateName)
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Duplicate));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "RUTA", this._exceptionFactory.Exceptions);
        }

        private void ValidatePrices(RouteModel model)
        {
            if (model.ProductRoutePrices == null)
                return;

            var invalidPrice = model.ProductRoutePrices.Any(item =>
                item.Price < 0 || (!model.DeliversRegalia && item.Price <= 0));

            if (invalidPrice)
            {
                this._exceptionFactory.Add("price", model.DeliversRegalia
                    ? "El precio no puede ser negativo"
                    : "El precio debe ser mayor a 0");
            }

            if (model.ProductRoutePrices.Any(item => item.CreditPrice.HasValue && item.CreditPrice.Value <= 0))
                this._exceptionFactory.Add("creditPrice", "El precio de crédito tiene que ser mayor a 0");
        }

        private static double? PositiveCreditPrice(double? creditPrice)
        {
            if (!creditPrice.HasValue || creditPrice.Value <= 0)
                return null;

            return creditPrice;
        }
    }
}
