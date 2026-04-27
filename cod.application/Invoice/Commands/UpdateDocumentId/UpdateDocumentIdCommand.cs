using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;
using cod.domain.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Invoice.Commands.UpdateDocumentId
{
    public class UpdateDocumentIdCommand : BaseCommand, IUpdateDocumentIdCommand
    {
        public UpdateDocumentIdCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(UpdateDocumentIdModel model)
        {
            var invoice = await _repository.Query<domain.Operation.Invoice>()
                .Where(f => f.Id == model.Id).FirstOrDefaultAsync();


            /*invoice.HasFile = true;
            invoice.DocumentId = model.DocumentId;
            invoice.FileUploadedUser = model.User;
            invoice.FileUploadedAt = Clock.Now;*/

            _repository.UpdateSync(invoice);

            await _repository.CommitAsync();
        }
    }
}
