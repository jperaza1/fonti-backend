using System.Threading.Tasks;

namespace cod.application.Tenant.Commands.ProfileTenant
{
    public interface IProfileTenantCommand
    {
        Task Execute(ProfileTenantModel model, int id);
    }
}
