using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.application.Invoice.Commands.UpdateDocumentId;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Expense.Commands.UpdateExpenseDocumentId
{
    public class UpdateExpenseDocumentIdCommand : BaseCommand, IUpdateExpenseDocumentIdCommand
    {
        public UpdateExpenseDocumentIdCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(UpdateDocumentIdModel model)
        {
            var deposit = await _repository.Query<domain.Operation.Expense>()
                .Where(f => f.Id == model.Id).FirstOrDefaultAsync();

            deposit.DocumentId = model.DocumentId;
            deposit.DocumentUploadedUser = model.User;
            deposit.DocumentUploadedAt = Clock.Now;

            _repository.UpdateSync(deposit);

            await _repository.CommitAsync();
        }
    }
}
