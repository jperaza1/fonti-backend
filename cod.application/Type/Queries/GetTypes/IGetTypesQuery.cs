using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.Type.Queries.GetTypes
{
    public interface IGetTypesQuery
    {
        Task<List<GetTypeModel>> Execute();
    }
}
