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
            route.Order = model.Order;
            route.DeliversRegalia = model.DeliversRegalia;

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
                    Price = item.Price
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

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "RUTA", this._exceptionFactory.Exceptions);
        }
    }
}
