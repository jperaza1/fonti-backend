using System.Threading.Tasks;

namespace cod.application.Tenant.Queries.GetCurrentTenant
{
    public interface IGetCurrentTenantQuery
    {
        Task<GetCurrentTenantModel> Execute();
    }
}
