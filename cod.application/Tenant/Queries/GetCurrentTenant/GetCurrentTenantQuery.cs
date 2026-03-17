using System.Threading.Tasks;
using cod.domain.Authorization;
using cod.domain.Features;
using cod.application.Features.Base;
using cod.application.Features.Extensions;
using cod.application.Interfaces;

namespace cod.application.Tenant.Queries.GetCurrentTenant
{
    public class GetCurrentTenantQuery : BaseQuery, IGetCurrentTenantQuery
    {
        private readonly AuthService _authService;

        public GetCurrentTenantQuery(IRepository repository,
            AuthService authService) : base(repository)
        {
            _authService = authService;
        }

        public async Task<GetCurrentTenantModel> Execute()
        {

            ApplicationTenant tenant = await
                _repository
                    .FirstOrDefaultAsync<ApplicationTenant>(company => company.Id == _authService.TenantId);
            if (tenant.IsNull())
                return new GetCurrentTenantModel();
            return new GetCurrentTenantModel
            {
                Name = tenant.Name,
                Address = tenant.Address,
                Logo = tenant.Logo?.ConvertToPictureString(),
                Phone = tenant.PhoneNumber?.ToPhoneFormat()
            };
        }
    }
}