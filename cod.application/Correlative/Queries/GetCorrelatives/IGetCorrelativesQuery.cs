using cod.application.Correlative.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.Correlative.Queries.GetCorrelatives
{
    public interface IGetCorrelativesQuery
    {
        Task<List<CorrelativeModel>> Execute();
    }
}
