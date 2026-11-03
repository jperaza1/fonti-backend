using System.Threading.Tasks;

namespace cod.application.UnitOfMeasure.Commands.DisableUnitOfMeasure
{
    public interface IDisableUnitOfMeasureCommand
    {
        Task Execute(int id);
    }
}
