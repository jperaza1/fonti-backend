using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Bank.Commands.EnableBank
{
    public class EnableBankCommand : BaseCommand, IEnableBankCommand
    {
        public EnableBankCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(int id)
        {
            var exist = await _repository.AnyAsync<cod.domain.Data.Bank>(x => x.Id == id);

            if (!exist)
                throw new ApplicationException("BANCO NO EXISTE", "BANCO");

            await _repository.UpdateAsync<cod.domain.Data.Bank>(
                type => type.Id == id,
                type => new cod.domain.Data.Bank
                {
                    IsActive = true
                });

            await _repository.CommitAsync();
        }
    }
}
