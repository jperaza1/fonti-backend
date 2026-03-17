using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cod.domain.Authorization;
using Microsoft.EntityFrameworkCore;
using cod.application.Interfaces;

namespace cod.application.Tenant.Queries.GetTenants
{
    public class GetTenantsQuery : IGetTenantsQuery
    {
        private readonly IRepository _repository;

        public GetTenantsQuery(IRepository repository)
        {
            _repository = repository;
        }
        public Task<List<GetTenantsModel>> Execute()
        {
            return _repository.Query<ApplicationTenant>().Select(tenant => new GetTenantsModel
            {
                Name = tenant.Name,
                Address = tenant.Address,
                Email = tenant.Email,
                PhoneNumber = tenant.PhoneNumber,
                Id = tenant.Id,
                IsActive = tenant.IsActive
            }).ToListAsync();
        }
    }
}