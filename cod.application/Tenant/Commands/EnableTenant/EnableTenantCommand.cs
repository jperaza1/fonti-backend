using System.Threading.Tasks;
using cod.domain.Authorization;
using cod.application.Features.Exception;
using cod.application.Interfaces;

namespace cod.application.Tenant.Commands.EnableTenant
{
    public class EnableTenantCommand : IEnableTenantCommand
    {
        private readonly IRepository _repository;
        private readonly IExceptionFactory _exceptionFactory;

        public EnableTenantCommand(IRepository repository,
            IExceptionFactory exceptionFactory)
        {
            _repository = repository;
            _exceptionFactory = exceptionFactory;
        }
        public async Task Execute(int id)
        {
            await Validate(id);
            await _repository.UpdateAsync<ApplicationTenant>(tenant => tenant.Id == id,
                newValuesTenants => new ApplicationTenant
                {
                    IsActive = true
                });

            await _repository.CommitAsync();
        }

        public async Task Validate(int id)
        {
            bool existTenant = await _repository
                .AnyAsync<ApplicationTenant>(tenant => tenant.Id == id);

            if (!existTenant)
                throw new ApplicationException("TENANT NOT FOUND", "TENANT");
        }
    }
}