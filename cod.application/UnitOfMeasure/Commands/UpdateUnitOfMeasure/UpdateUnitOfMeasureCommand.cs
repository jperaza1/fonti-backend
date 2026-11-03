using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.UnitOfMeasure.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.UnitOfMeasure.Commands.UpdateUnitOfMeasure
{
    public class UpdateUnitOfMeasureCommand : BaseCommand, IUpdateUnitOfMeasureCommand
    {
        public UpdateUnitOfMeasureCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(UnitOfMeasureModel model, int id)
        {
            await ValidateModel(model, id);

            var entity = await _repository.Query<domain.Data.UnitOfMeasure>()
                .Where(f => f.Id == id)
                .FirstOrDefaultAsync();

            if (entity == null)
                throw new ApplicationException("Unidad de medida no existe", "UNIDAD DE MEDIDA");

            entity.Name = model.Name.Trim();
            _repository.UpdateSync(entity);
            await _repository.CommitAsync();
        }

        private async Task ValidateModel(UnitOfMeasureModel model, int id)
        {
            if (model.Name.NullOrEmpty())
                _exceptionFactory.Add("name", _exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Required));

            if (!_exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "UNIDAD DE MEDIDA", _exceptionFactory.Exceptions);

            var duplicateName = await _repository.AnyAsync<domain.Data.UnitOfMeasure>(f => f.Name == model.Name.Trim() && f.Id != id);
            if (duplicateName)
                _exceptionFactory.Add("name", _exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Duplicate));

            if (!_exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "UNIDAD DE MEDIDA", _exceptionFactory.Exceptions);
        }
    }
}
