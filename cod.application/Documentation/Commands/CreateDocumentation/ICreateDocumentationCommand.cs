using System.Threading.Tasks;

namespace cod.application.Documentation.Commands.CreateDocumentation
{
    public interface ICreateDocumentationCommand
    {
        Task Execute(CreateDocumentationModel model);
    }
}
