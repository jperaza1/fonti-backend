using cod.application.Bank.Common;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Bank.Commands.UpdateBank
{
    public class UpdateBankCommand : BaseCommand, IUpdateBankCommand
    {
        public UpdateBankCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(BankModel model, int id)
        {
            await ValidateModel(model, id);

            var bank = await _repository.Query<cod.domain.Data.Bank>()
                .Where(f => f.Id == id)
                .FirstOrDefaultAsync();

            if (bank == null)
                throw new ApplicationException("Banco no existe", "BANCO");

            bank.Name = model.Name;
            bank.Code = model.Code;
            bank.Alias = model.Alias;

            _repository.UpdateSync(bank);

            await _repository.CommitAsync();
        }

        private async Task ValidateModel(BankModel model, int id)
        {
            if (model.Name.NullOrEmpty())
                _exceptionFactory.Add("name", _exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Required));

            if (model.Code.NullOrEmpty())
                _exceptionFactory.Add("code", _exceptionFactory.GetMessage("Código", ExceptionEnums.ExceptionCode.Required));

            if (!_exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "BANCO", _exceptionFactory.Exceptions);

            var duplicateName = await _repository
                .AnyAsync<cod.domain.Data.Bank>(f => f.Name == model.Name && f.Id != id);

            if (duplicateName)
                _exceptionFactory.Add("name", _exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Duplicate));

            var duplicateCode = await _repository
                .AnyAsync<cod.domain.Data.Bank>(f => f.Code == model.Code && f.Id != id);

            if (duplicateCode)
                _exceptionFactory.Add("code", _exceptionFactory.GetMessage("Código", ExceptionEnums.ExceptionCode.Duplicate));

            if (!_exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "BANCO", _exceptionFactory.Exceptions);
        }
    }
}
