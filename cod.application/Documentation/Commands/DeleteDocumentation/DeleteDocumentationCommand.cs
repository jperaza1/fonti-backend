using System.Threading.Tasks;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;

namespace cod.application.Documentation.Commands.DeleteDocumentation
{
    public class DeleteDocumentationCommand : BaseCommand, IDeleteDocumentationCommand
    {
        public DeleteDocumentationCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }

        public async Task Execute(string entity, int entityId, string documentType, string fileName)
        {

            domain.General.Documentation documentation =
                await _repository
                    .FirstOrDefaultActive<domain.General.Documentation>(doc =>
                        doc.Entity == entity
                        && doc.EntityId == entityId
                        && doc.DocumentType == documentType
                        && doc.FileName == fileName);

            _repository.DeleteSync(documentation);

            await _repository.CommitAsync();

        }
    }
}