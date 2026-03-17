using System.Collections.Generic;
using System.Threading.Tasks;

namespace cod.application.Tenant.Queries.GetTenants
{
    public interface IGetTenantsQuery
    {
        Task<List<GetTenantsModel>> Execute();
    }
}
