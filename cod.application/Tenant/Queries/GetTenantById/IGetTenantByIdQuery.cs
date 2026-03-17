using System.Threading.Tasks;

namespace cod.application.Tenant.Queries.GetTenantById
{
    public interface IGetTenantByIdQuery
    {
        Task<GetTenantByIdModel> Execute(int id);
    }
}
