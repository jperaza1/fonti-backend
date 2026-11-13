using cod.application.Common;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Route.Common;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Route.Commands.UpdateRoute
{
    public class UpdateRouteCommand : BaseCommand, IUpdateRouteCommand
    {
        private readonly ILogHelper _logHelper;
        public UpdateRouteCommand(
            IRepository repository, 
            IExceptionFactory exceptionFactory,
            ILogHelper logHelper) : base(repository, exceptionFactory)
        {
            _logHelper = logHelper;
        }

        public async Task Execute(RouteModel model, int id)
        {
            await ValidateModel(model);
            _logHelper.Initialize(_repository);
            List<CompareModel> logs = new List<CompareModel>();

            var route = await _repository.Query<domain.Data.Route>().Where(f => f.Id == id).FirstOrDefaultAsync();

            if (route == null) throw new ApplicationException("Ruta no existe", "RUTA", this._exceptionFactory.Exceptions);

            route.Name = model.Name;
            route.Vendor = model.Vendor;
            route.Order = model.Order;
            route.DeliversRegalia = model.DeliversRegalia;
            route.GivesCredit = model.GivesCredit;

            _repository.UpdateSync(route);

            var details = await _repository.AllActive<domain.Data.ProductRoutePrice>().Where(f => f.RouteId == id).ToListAsync();

            var detailsToRemove = details.Where(detail => model.ProductRoutePrices.All(member => member.Id != detail.Id)).ToList();

            foreach (var item in detailsToRemove)
            {
                _repository.InsertAsync(new domain.General.DataChangeLog
                {
                    Date = Clock.Now,
                    Entity = "ROUTE",
                    EntityId = id,
                    Log = $"(Elimando Producto) => {item.Product.Name} - Precio: {item.Price}"
                });
                _repository.DeleteSync(item);
            }

            var detailsToUpdate = model.ProductRoutePrices.Where(dd => dd.Id > 0).ToList();

            foreach (var item in detailsToUpdate)
            {
                var existing = details.FirstOrDefault(detail => detail.Id == item.Id);
                if (existing == null)
                    continue;

                if (item.ShowCreditPrice.HasValue)
                {
                    var show = item.ShowCreditPrice.Value && existing.Price > 0;
                    double? nextCredit = show ? existing.Price : null;
                    if (existing.ShowCreditPrice == show && existing.CreditPrice == nextCredit)
                        continue;

                    _repository.InsertAsync(new domain.General.DataChangeLog
                    {
                        Date = Clock.Now,
                        Entity = "ROUTE",
                        EntityId = id,
                        Log = $"(Mostrar precio al crédito) => {item.ProductName}: {(show ? existing.Price.ToString() : "no")}"
                    });
                    existing.ShowCreditPrice = show;
                    existing.CreditPrice = nextCredit;
                    _repository.UpdateSync(existing);
                    continue;
                }

                if (!item.CreditPrice.HasValue || item.CreditPrice.Value <= 0)
                    continue;

                if (existing.CreditPrice == item.CreditPrice && existing.ShowCreditPrice)
                    continue;

                _repository.InsertAsync(new domain.General.DataChangeLog
                {
                    Date = Clock.Now,
                    Entity = "ROUTE",
                    EntityId = id,
                    Log = $"(Precio de crédito) => {item.ProductName}: {existing.CreditPrice} -> {item.CreditPrice}"
                });
                existing.ShowCreditPrice = true;
                existing.CreditPrice = item.CreditPrice;
                _repository.UpdateSync(existing);
            }

            var detailToAdd = model.ProductRoutePrices.Where(dd => dd.Id <= 0).ToList();

            foreach (var item in detailToAdd)
            {
                _repository.InsertAsync(new domain.General.DataChangeLog
                {
                    Date = Clock.Now,
                    Entity = "ROUTE",
                    EntityId = id,
                    Log = $"(Agregando Producto) => {item.ProductName} - Precio: {item.Price}"
                });
                _repository.InsertAsync(new domain.Data.ProductRoutePrice
                {
                    ProductId = item.ProductId,
                    RouteId = id,
                    Price = item.Price,
                    ShowCreditPrice = item.ShowCreditPrice == true && item.Price > 0,
                    CreditPrice = item.ShowCreditPrice == true && item.Price > 0
                        ? item.Price
                        : item.CreditPrice.HasValue && item.CreditPrice.Value > 0
                            ? item.CreditPrice
                            : null
                });
            }
            await _repository.CommitAsync();
        }

        public async Task ValidateModel(RouteModel model)
        {
            if (model.Name.NullOrEmpty())
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Required));

            if (model.ProductRoutePrices != null &&
                model.ProductRoutePrices.Any(item => item.Price < 0 || (!model.DeliversRegalia && item.Price <= 0)))
            {
                this._exceptionFactory.Add("price", model.DeliversRegalia
                    ? "El precio no puede ser negativo"
                    : "El precio debe ser mayor a 0");
            }

            if (model.ProductRoutePrices != null &&
                model.ProductRoutePrices.Any(item => item.CreditPrice.HasValue && item.CreditPrice.Value <= 0))
            {
                this._exceptionFactory.Add("creditPrice", "El precio de crédito tiene que ser mayor a 0");
            }

            if (model.ProductRoutePrices != null &&
                model.ProductRoutePrices.Any(item => item.ShowCreditPrice == true && item.Price <= 0))
            {
                this._exceptionFactory.Add("showCreditPrice", "El precio que se muestra al crédito tiene que ser mayor a 0");
            }

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "RUTA", this._exceptionFactory.Exceptions);
        }
    }
}
