using System.Threading.Tasks;

namespace cod.application.Documentation.Commands.DeleteDocumentation
{
    public interface IDeleteDocumentationCommand
    {
        Task Execute(string entity, int entityId, string documentType, string fileName);
    }
}
