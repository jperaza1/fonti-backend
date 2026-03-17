using System;
using cod.application.Tenant.Commands.UpdateTenant;

namespace cod.application.Tenant.Queries.GetTenants
{
    public class GetTenantsModel : UpdateTenantModel
    {
        public string City { get; set; }
        public bool Demo { get; set; }
        public DateTime? DemoTerminationDate { get; set; }
        public bool IsActive { get; set; }
    }
}
