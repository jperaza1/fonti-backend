using cod.application.UnitOfMeasure.Common;
using System.Threading.Tasks;

namespace cod.application.UnitOfMeasure.Commands.CreateUnitOfMeasure
{
    public interface ICreateUnitOfMeasureCommand
    {
        Task Execute(UnitOfMeasureModel model);
    }
}
