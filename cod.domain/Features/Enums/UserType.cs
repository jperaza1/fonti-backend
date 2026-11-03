using System.ComponentModel;

namespace cod.domain.Features.Enums
{
    public partial class DomainEnums
    {
        public enum UserType
        {
            SuperAdmin = 1,
            Administrativo = 2,
            General = 3,
            [Description("Vendedor")]
            Vendor = 4
        }
    }
}
