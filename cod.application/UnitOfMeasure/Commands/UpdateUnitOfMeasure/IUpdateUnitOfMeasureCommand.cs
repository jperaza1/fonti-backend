using cod.application.UnitOfMeasure.Common;
using System.Threading.Tasks;

namespace cod.application.UnitOfMeasure.Commands.UpdateUnitOfMeasure
{
    public interface IUpdateUnitOfMeasureCommand
    {
        Task Execute(UnitOfMeasureModel model, int id);
    }
}
