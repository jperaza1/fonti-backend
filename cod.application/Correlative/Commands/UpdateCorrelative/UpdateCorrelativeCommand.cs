using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Correlative.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Correlative.Commands.UpdateCorrelative
{
    public class UpdateCorrelativeCommand : BaseCommand, IUpdateCorrelativeCommand
    {
        public UpdateCorrelativeCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(CorrelativeModel model, int id)
        {
            ValidateModel(model);

            var correlative = await _repository.Query<domain.Data.Correlative>().Where(f => f.Id == id).FirstOrDefaultAsync();

            if (correlative == null) throw new ApplicationException("Correlativo no existe", "CORRELATIVO", this._exceptionFactory.Exceptions);

            correlative.Cai = model.Cai;
            correlative.Start = model.Start;
            correlative.End = model.End;
            correlative.StartDate = model.StartDate;
            correlative.EndDate = model.EndDate;
            correlative.Current = model.Current;
            correlative.EmissionPoint = model.EmissionPoint;
            correlative.Establishment = model.Establishment;
            correlative.DocumentType = model.DocumentType;

            _repository.UpdateSync(correlative);

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
