using cod.application.BankAccount.Common;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.BankAccount.Commands.UpdateBankAccount
{
    public class UpdateBankAccountCommand : BaseCommand, IUpdateBankAccountCommand
    {
        public UpdateBankAccountCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(BankAccountModel model, int id)
        {
            await ValidateModel(model, id);

            var account = await _repository.Query<cod.domain.Finance.BankAccount>()
                .Where(f => f.Id == id)
                .FirstOrDefaultAsync();

            if (account == null)
                throw new ApplicationException("Cuenta bancaria no existe", "CUENTA_BANCARIA");

            account.Name = model.Name;
            account.BankId = model.BankId;

            _repository.UpdateSync(account);

            await _repository.CommitAsync();
        }

        private async Task ValidateModel(BankAccountModel model, int id)
        {
            if (model.Name.NullOrEmpty())
                _exceptionFactory.Add("name", _exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Required));

            if (model.BankId <= 0)
                _exceptionFactory.Add("bank", _exceptionFactory.GetMessage("Banco", ExceptionEnums.ExceptionCode.Required));

            if (!_exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "CUENTA_BANCARIA", _exceptionFactory.Exceptions);

            var duplicateName = await _repository
                .AnyAsync<cod.domain.Finance.BankAccount>(f => f.Name == model.Name && f.Id != id);

            if (duplicateName)
                _exceptionFactory.Add("name", _exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Duplicate));

            if (!_exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "CUENTA_BANCARIA", _exceptionFactory.Exceptions);
        }
    }
}
