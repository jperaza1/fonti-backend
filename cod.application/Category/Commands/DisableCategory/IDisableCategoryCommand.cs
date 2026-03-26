using System.Threading.Tasks;

namespace cod.application.Category.Commands.DisableCategory
{
    public interface IDisableCategoryCommand
    {
        Task Execute(int id);
    }
}
