using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.BankAccount.Commands.EnableBankAccount
{
    public class EnableBankAccountCommand : BaseCommand, IEnableBankAccountCommand
    {
        public EnableBankAccountCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(int id)
        {
            var exist = await _repository.AnyAsync<cod.domain.Finance.BankAccount>(x => x.Id == id);

            if (!exist)
                throw new ApplicationException("CUENTA BANCARIA NO EXISTE", "CUENTA_BANCARIA");

            await _repository.UpdateAsync<cod.domain.Finance.BankAccount>(
                type => type.Id == id,
                type => new cod.domain.Finance.BankAccount
                {
                    IsActive = true
                });

            await _repository.CommitAsync();
        }
    }
}
