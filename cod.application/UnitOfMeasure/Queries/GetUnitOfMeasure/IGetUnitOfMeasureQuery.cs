using cod.application.UnitOfMeasure.Common;
using System.Threading.Tasks;

namespace cod.application.UnitOfMeasure.Queries.GetUnitOfMeasure
{
    public interface IGetUnitOfMeasureQuery
    {
        Task<UnitOfMeasureModel> Execute(int id);
    }
}
