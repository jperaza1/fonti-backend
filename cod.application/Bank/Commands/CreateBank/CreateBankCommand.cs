using cod.application.Bank.Common;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.domain.Features;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Bank.Commands.CreateBank
{
    public class CreateBankCommand : BaseCommand, ICreateBankCommand
    {
        public CreateBankCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(BankModel model)
        {
            await ValidateModel(model);

            var entity = new cod.domain.Data.Bank
            {
                Name = model.Name,
                Code = model.Code,
                Alias = model.Alias
            };

            _repository.InsertAsync(entity);

            await _repository.CommitAsync();
        }

        private async Task ValidateModel(BankModel model)
        {
            if (model.Name.NullOrEmpty())
                _exceptionFactory.Add("name", _exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Required));

            if (model.Code.NullOrEmpty())
                _exceptionFactory.Add("code", _exceptionFactory.GetMessage("Código", ExceptionEnums.ExceptionCode.Required));

            if (!_exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "BANCO", _exceptionFactory.Exceptions);

            var duplicateName = await _repository
                .AnyAsync<cod.domain.Data.Bank>(f => f.Name == model.Name);

            if (duplicateName)
                _exceptionFactory.Add("name", _exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Duplicate));

            var duplicateCode = await _repository
                .AnyAsync<cod.domain.Data.Bank>(f => f.Code == model.Code);

            if (duplicateCode)
                _exceptionFactory.Add("code", _exceptionFactory.GetMessage("Código", ExceptionEnums.ExceptionCode.Duplicate));

            if (!_exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "BANCO", _exceptionFactory.Exceptions);
        }
    }
}
