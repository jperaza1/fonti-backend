using System.Collections.Generic;

namespace cod.application.Tenant.Commands.CreateTenant
{
    public class CreateTenantModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Address { get; set; }
        public string Suite { get; set; }
        public int? CityId { get; set; }
        public string ZipCode { get; set; }
        public string WebSite { get; set; }
        public string Logo { get; set; }
        public List<string> Permissions { get; set; }
    }
}