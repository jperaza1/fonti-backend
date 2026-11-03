using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.UnitOfMeasure.Common;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.UnitOfMeasure.Commands.CreateUnitOfMeasure
{
    public class CreateUnitOfMeasureCommand : BaseCommand, ICreateUnitOfMeasureCommand
    {
        public CreateUnitOfMeasureCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(UnitOfMeasureModel model)
        {
            await ValidateModel(model);

            var entity = new domain.Data.UnitOfMeasure
            {
                Name = model.Name.Trim()
            };

            _repository.InsertAsync(entity);
            await _repository.CommitAsync();
        }

        private async Task ValidateModel(UnitOfMeasureModel model)
        {
            if (model.Name.NullOrEmpty())
                _exceptionFactory.Add("name", _exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Required));

            if (!_exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "UNIDAD DE MEDIDA", _exceptionFactory.Exceptions);

            var duplicateName = await _repository.AnyAsync<domain.Data.UnitOfMeasure>(f => f.Name == model.Name.Trim());
            if (duplicateName)
                _exceptionFactory.Add("name", _exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Duplicate));

            if (!_exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "UNIDAD DE MEDIDA", _exceptionFactory.Exceptions);
        }
    }
}
