using System.Threading.Tasks;

namespace cod.application.Category.Commands.EnableCategory
{
    public interface IEnableCategoryCommand
    {
        Task Execute(int id);
    }
}
