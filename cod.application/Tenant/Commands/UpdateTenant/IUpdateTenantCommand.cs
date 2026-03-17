using System.Threading.Tasks;

namespace cod.application.Tenant.Commands.UpdateTenant
{
    public interface IUpdateTenantCommand
    {
        Task Execute(UpdateTenantModel model, int id);
    }
}
