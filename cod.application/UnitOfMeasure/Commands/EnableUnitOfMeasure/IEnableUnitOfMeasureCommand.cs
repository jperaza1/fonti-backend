using System.Threading.Tasks;

namespace cod.application.UnitOfMeasure.Commands.EnableUnitOfMeasure
{
    public interface IEnableUnitOfMeasureCommand
    {
        Task Execute(int id);
    }
}
