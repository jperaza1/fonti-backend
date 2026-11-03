using cod.application.UnitOfMeasure.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.UnitOfMeasure.Queries.GetActiveUnitOfMeasures
{
    public interface IGetActiveUnitOfMeasuresQuery
    {
        Task<List<UnitOfMeasureModel>> Execute();
    }
}
