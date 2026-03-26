using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Correlative.Common;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Correlative.Commands.CreateCorrelative
{
    public class CreateCorrelativeCommand : BaseCommand, ICreateCorrelativeCommand
    {
        public CreateCorrelativeCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(CorrelativeModel model)
        {
            ValidateModel(model);

            var newCorrelative = new domain.Data.Correlative
            {
                Cai = model.Cai,
                Start = model.Start,
                End = model.End,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Current = model.Current,
                EmissionPoint = model.EmissionPoint,
                Establishment = model.Establishment,
                DocumentType = model.DocumentType,
            };

            _repository.InsertAsync(newCorrelative);

            await _repository.CommitAsync();
        }

        public void ValidateModel(CorrelativeModel model)
        {
            if (model.Cai.NullOrEmpty())
                this._exceptionFactory.Add("cai", this._exceptionFactory.GetMessage("CAI", ExceptionEnums.ExceptionCode.Required));

            if (model.EmissionPoint.NullOrEmpty())
                this._exceptionFactory.Add("emissionPoint", this._exceptionFactory.GetMessage("Punto de Emisión", ExceptionEnums.ExceptionCode.Required));

            if (model.Establishment.NullOrEmpty())
                this._exceptionFactory.Add("establishment", this._exceptionFactory.GetMessage("Establecimiento", ExceptionEnums.ExceptionCode.Required));

            if (model.DocumentType.NullOrEmpty())
                this._exceptionFactory.Add("documentType", this._exceptionFactory.GetMessage("Tipo de Documento", ExceptionEnums.ExceptionCode.Required));

            if (!this._exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "CORRELATIVO", this._exceptionFactory.Exceptions);
        }
    }
}
