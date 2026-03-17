using System.Threading.Tasks;

namespace cod.application.Tenant.Commands.EnableTenant
{
    public interface IEnableTenantCommand
    {
        Task Execute(int id);
    }
}
