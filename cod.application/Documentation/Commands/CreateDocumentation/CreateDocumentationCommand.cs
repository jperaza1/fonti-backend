using System.Threading.Tasks;
using cod.application.Features.Base;
using cod.application.Features.Exception;
using cod.application.Interfaces;

namespace cod.application.Documentation.Commands.CreateDocumentation
{
    public class CreateDocumentationCommand : BaseCommand, ICreateDocumentationCommand
    {
        public CreateDocumentationCommand(IRepository repository, IExceptionFactory exceptionFactory) : base(repository, exceptionFactory)
        {
        }
        public async Task Execute(CreateDocumentationModel model)
        {
            domain.General.Documentation documentation = new domain.General.Documentation
            {
                Entity = model.Entity,
                EntityId = model.EntityId,
                DocumentType = model.DocumentType,
                FileName = model.FileName,
                ExternalId = model.ExternalId
            };
            _repository.InsertAsync(documentation);

            await _repository.CommitAsync();
        }
    }
}