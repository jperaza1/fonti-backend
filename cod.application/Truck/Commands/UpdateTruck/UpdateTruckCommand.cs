using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Truck.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Truck.Commands.UpdateTruck
{
    public class UpdateTruckCommand : BaseCommand, IUpdateTruckCommand
    {
        public UpdateTruckCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(TruckModel model, int id)
        {
            await ValidateModel(model, id);

            var truck = await _repository.Query<domain.Data.Truck>().Where(f => f.Id == id).FirstOrDefaultAsync();

            if (truck == null)
                throw new ApplicationException("Camión no existe", "CAMION", this._exceptionFactory.Exceptions);

            truck.Name = model.Name;
            truck.Plate = model.Plate;
            truck.TruckStatus = model.TruckStatus;
            truck.RouteId = model.RouteId;

            _repository.UpdateSync(truck);

            await _repository.CommitAsync();
        }

        public async Task ValidateModel(TruckModel model, int id)
        {
            if (model.Name.NullOrEmpty())
                this._exceptionFactory.Add("name", this._exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Required));

            if (model.Plate.NullOrEmpty())
                this._exceptionFactory.Add("plate", this._exceptionFactory.GetMessage("Placa", ExceptionEnums.ExceptionCode.Required));

            if (!model.TruckStatus.IsValid())
                this._exceptionFactory.Add("truckStatus", this._exceptionFactory.GetMessage("Estado", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "CAMION", this._exceptionFactory.Exceptions);

            bool duplicatePlate = await _repository
                .AnyAsync<domain.Data.Truck>(f => f.Plate.ToUpper() == model.Plate.ToUpper() && f.Id != id);

            if (duplicatePlate)
                this._exceptionFactory.Add("plate", this._exceptionFactory.GetMessage("Placa", ExceptionEnums.ExceptionCode.Duplicate));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "CAMION", this._exceptionFactory.Exceptions);
        }
    }
}
