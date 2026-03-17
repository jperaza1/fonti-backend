using System.Threading.Tasks;

namespace cod.application.Tenant.Commands.CreateTenant
{
    public interface ICreateTenantCommand
    {
        Task Execute(CreateTenantModel createTenantModel);
    }
}
