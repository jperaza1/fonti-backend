using cod.application.Kardex.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.Kardex.Queries.GetKardex
{
    public interface IGetKardexQuery
    {
        Task<List<KardexModel>> Execute(KardexFilterModel filter);
    }
}
