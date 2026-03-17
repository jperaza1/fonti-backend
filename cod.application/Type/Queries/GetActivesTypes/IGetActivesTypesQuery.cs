using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.Type.Queries.GetActivesTypes
{
    public interface IGetActivesTypesQuery
    {
        Task<List<GetActivesTypesModel>> Execute();
    }
}
