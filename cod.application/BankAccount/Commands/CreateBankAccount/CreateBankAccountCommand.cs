using cod.application.BankAccount.Common;
using cod.application.Common.Extensions;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.domain.Features;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.BankAccount.Commands.CreateBankAccount
{
    public class CreateBankAccountCommand : BaseCommand, ICreateBankAccountCommand
    {
        private readonly AuthService _authService;

        public CreateBankAccountCommand(
            IRepository repository,
            IExceptionFactory exceptionFactory,
            AuthService authService) : base(repository, exceptionFactory)
        {
            _authService = authService;
        }

        public async Task Execute(BankAccountModel model)
        {
            await ValidateModel(model);

            var entity = new cod.domain.Finance.BankAccount
            {
                Name = model.Name,
                BankId = model.BankId,
                Balance = 0,
            };

            _repository.InsertAsync(entity);

            await _repository.CommitAsync();
        }

        private async Task ValidateModel(BankAccountModel model)
        {
            if (model.Name.NullOrEmpty())
                _exceptionFactory.Add("name", _exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Required));

            if (model.BankId <= 0)
                _exceptionFactory.Add("bank", _exceptionFactory.GetMessage("Banco", ExceptionEnums.ExceptionCode.Required));

            if (!_exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "CUENTA_BANCARIA", _exceptionFactory.Exceptions);

            var duplicateName = await _repository
                .AnyAsync<cod.domain.Finance.BankAccount>(f => f.Name == model.Name);

            if (duplicateName)
                _exceptionFactory.Add("name", _exceptionFactory.GetMessage("Nombre", ExceptionEnums.ExceptionCode.Duplicate));

            if (!_exceptionFactory.IsValid())
                throw new ApplicationException("CANCELED", "CUENTA_BANCARIA", _exceptionFactory.Exceptions);
        }
    }
}
