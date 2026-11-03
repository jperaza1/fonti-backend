using cod.application.UnitOfMeasure.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.UnitOfMeasure.Queries.GetUnitOfMeasures
{
    public interface IGetUnitOfMeasuresQuery
    {
        Task<List<UnitOfMeasureModel>> Execute();
    }
}
