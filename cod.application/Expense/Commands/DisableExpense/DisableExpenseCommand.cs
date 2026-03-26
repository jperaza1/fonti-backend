using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using System.Threading.Tasks;
using ApplicationException = cod.application.Features.Exception.ApplicationException;

namespace cod.application.Expense.Commands.DisableExpense
{
    public class DisableExpenseCommand : BaseCommand, IDisableExpenseCommand
    {
        public DisableExpenseCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(int id)
        {
            var exist = await _repository.AnyAsync<domain.Operation.Expense>(x => x.Id == id);

            if (!exist)
                throw new ApplicationException("GASTO NO EXISTE", "GASTO");

            await _repository.UpdateAsync<domain.Operation.Expense>(
                type => type.Id == id,
                type => new domain.Operation.Expense
                {
                    IsActive = false
                });

            await _repository.CommitAsync();
        }
    }
}
