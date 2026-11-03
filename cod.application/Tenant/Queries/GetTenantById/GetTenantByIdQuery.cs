using System.Linq;
using System.Threading.Tasks;
using cod.application.Features.Extensions;
using cod.domain.Authorization;
using cod.application.Interfaces;

namespace cod.application.Tenant.Queries.GetTenantById
{
    public class GetTenantByIdQuery : IGetTenantByIdQuery
    {
        private readonly IRepository _repository;

        public GetTenantByIdQuery(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<GetTenantByIdModel> Execute(int id)
        {
            var tenant =
                await _repository.FirstOrDefaultAsync<ApplicationTenant>(searchTenant => searchTenant.Id == id);
            return new GetTenantByIdModel
            {
                Name = tenant.Name,
                CompanyName = tenant.CompanyName,
                Rtn = tenant.Rtn,
                Address = tenant.Address,
                Email = tenant.Email,
                PhoneNumber = tenant.PhoneNumber,
                Id = tenant.Id,
                Logo = tenant.Logo?.ConvertToPictureString(),
                IsActive = tenant.IsActive,
                Permissions = tenant.SosTenantPermissions.Select(permission => permission.PermissionId).ToList()
            };
        }
    }
}