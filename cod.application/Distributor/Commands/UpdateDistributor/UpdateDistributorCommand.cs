using cod.application.Common.Extensions;
using cod.application.Distributor.Common;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Distributor.Commands.UpdateDistributor
{
    public class UpdateDistributorCommand : BaseCommand, IUpdateDistributorCommand
    {
        public UpdateDistributorCommand(
            IRepository repository,
            IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(DistributorModel model, int id)
        {
            await ValidateModel(model);

            var distributor = await _repository.Query<domain.Data.Distributor>().Where(f => f.Id == id).FirstOrDefaultAsync();

            if (distributor == null) throw new ApplicationException("Distribuidor no existe", "DISTRIBUIDOR", this._exceptionFactory.Exceptions);

            distributor.Name = model.Name;

            _repository.UpdateSync(distributor);

            var details = await _repository.AllActive<domain.Data.ProductDistributorPrice>().Where(f => f.DistributorId == id).ToListAsync();
            var incomingDetails = model.ProductDistributorPrices ?? new List<ProductDistributorPricesModel>();

            var detailsToRemove = details.Where(detail => incomingDetails.All(member => member.Id != detail.Id)).ToList();

            foreach (var item in detailsToRemove)
            {
                _repository.InsertAsync(new domain.General.DataChangeLog
                {
                    Date = Clock.Now,
                    Entity = "DISTRIBUTOR",
                    EntityId = id,
                    Log = $"(Eliminando Producto) => {item.Product.Name} - Precio: {item.Price}"
                });
                _repository.DeleteSync(item);
            }

            var detailsToUpdate = incomingDetails.Where(dd => dd.Id > 0).ToList();

            foreach (var item in detailsToUpdate)
            {
                var existing = details.FirstOrDefault(d => d.Id == item.Id);
                if (existing == null || existing.Price == item.Price)
                    continue;

                _repository.InsertAsync(new domain.General.DataChangeLog
                {
                    Date = Clock.Now,
                    Entity = "DISTRIBUTOR",
                    EntityId = id,
                    Log = $"(Actualizando Producto) => {item.ProductName} - Precio: {existing.Price} -> {item.Price}"
                });
                existing.Price = item.Price;
                _repository.UpdateSync(existing);
            }

            var detailToAdd = incomingDetails.Where(dd => dd.Id <= 0).ToList();

            foreach (var item in detailToAdd)
            {
                _repository.InsertAsync(new domain.General.DataChangeLog
                {
                    Date = Clock.Now,
                    Entity = "DISTRIBUTOR",
                    EntityId = id,
                    Log = $"(Agregando Producto) => {item.ProductName} - Precio: {item.Price}"
                });
                _repository.InsertAsync(new domain.Data.ProductDistributorPrice
                {
                    ProductId = item.ProductId,
                    DistributorId = id,
                    Price = item.Price
                });
            }
            await _repository.CommitAsync();
        }

        public async Task ValidateModel(DistributorModel model)
        {
            if (model.Name.NullOrEmpty())
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "DISTRIBUIDOR", this._exceptionFactory.Exceptions);
        }
    }
}
