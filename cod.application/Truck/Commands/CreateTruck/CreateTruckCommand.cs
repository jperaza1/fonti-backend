using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Truck.Common;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Truck.Commands.CreateTruck
{
    public class CreateTruckCommand : BaseCommand, ICreateTruckCommand
    {
        public CreateTruckCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(TruckModel model)
        {
            await ValidateModel(model);

            if(model.TruckStatus == domain.Features.Enums.DomainEnums.TruckStatus.Maintenance)
            {
                model.RouteId = null;
            }

            var newTruck = new domain.Data.Truck
            {
                Name = model.Name,
                Plate = model.Plate,
                TruckStatus = model.TruckStatus,
                RouteId = model.RouteId,
            };

            _repository.InsertAsync(newTruck);

            await _repository.CommitAsync();
        }

        public async Task ValidateModel(TruckModel model)
        {
            if (model.Name.NullOrEmpty())
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Required));

            if (model.Plate.NullOrEmpty())
                this._exceptionFactory.Add("plate", this._exceptionFactory.GetMessage("Placa", ExceptionEnums.ExceptionCode.Required));

            if (!model.TruckStatus.IsValid())
                this._exceptionFactory.Add("truckStatus", this._exceptionFactory.GetMessage("Estado", ExceptionEnums.ExceptionCode.Required));

            if(model.TruckStatus == domain.Features.Enums.DomainEnums.TruckStatus.Available && !model.RouteId.HasValue)
                this._exceptionFactory.Add("routeId", this._exceptionFactory.GetMessage("Ruta", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "CAMION", this._exceptionFactory.Exceptions);

            bool duplicatePlate = await _repository
                .AnyAsync<domain.Data.Truck>(f => f.Plate.ToUpper() == model.Plate.ToUpper());

            if (duplicatePlate)
                this._exceptionFactory.Add("plate", this._exceptionFactory.GetMessage("Placa", ExceptionEnums.ExceptionCode.Duplicate));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "CAMION", this._exceptionFactory.Exceptions);
        }
    }
}
