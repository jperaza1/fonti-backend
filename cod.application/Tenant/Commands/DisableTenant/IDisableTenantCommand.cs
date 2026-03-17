using System.Threading.Tasks;

namespace cod.application.Tenant.Commands.DisableTenant
{
    public interface IDisableTenantCommand
    {
        Task Execute(int id);
    }
}
